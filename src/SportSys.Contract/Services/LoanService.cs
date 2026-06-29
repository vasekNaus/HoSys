using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using SportSys.Database.Models.inventory;

namespace SportSys.Contract.Services;

public class LoanService
{
    private readonly SportSysDbContext _db;

    public LoanService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<LoanListItem>> GetLoansAsync(LoanFilter filter, CancellationToken ct = default)
    {
        var query = _db.Loans.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.MemberName))
        {
            query = query.Where(l => (l.Member.DisplayName ?? l.Member.UserName ?? "")
                .Contains(filter.MemberName));
        }

        if (filter.DateFrom.HasValue)
            query = query.Where(l => l.LoanDate >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(l => l.LoanDate <= filter.DateTo.Value);

        if (filter.ActiveOnly)
            query = query.Where(l => l.ReturnedDate == null && !l.IsClosed);

        var loans = await query
            .Select(l => new
            {
                l.Id,
                l.MemberId,
                l.LoanDate,
                l.ReturnedDate,
                l.IsClosed,
                MemberName = l.Member.DisplayName ?? l.Member.UserName ?? "",
            })
            .ToListAsync(ct);

        // Grupování dle MemberId + LoanDate
        var groups = loans
            .GroupBy(l => new { l.MemberId, l.LoanDate })
            .OrderByDescending(g => g.Key.LoanDate)
            .Select(g =>
            {
                var groupId = g.Min(l => l.Id);
                var itemCount = g.Count();
                var returnedCount = g.Count(l => l.ReturnedDate != null || l.IsClosed);
                var status = returnedCount == 0 ? "Aktivní"
                    : returnedCount == itemCount ? "Uzavřeno"
                    : "Částečně vráceno";

                return new LoanListItem
                {
                    GroupId = groupId,
                    LoanNumber = $"V-{groupId:D5}",
                    MemberName = g.First().MemberName,
                    LoanDate = g.Key.LoanDate,
                    ItemCount = itemCount,
                    ReturnedCount = returnedCount,
                    Status = status,
                };
            })
            .ToList();

        return groups;
    }

    public async Task<LoanDetail?> GetLoanDetailAsync(int groupId, CancellationToken ct = default)
    {
        // Najdi skupinový záznam dle groupId (= min Id)
        var pivotLoan = await _db.Loans
            .Where(l => l.Id == groupId)
            .Select(l => new { l.MemberId, l.LoanDate, l.ExpectedReturnDate })
            .FirstOrDefaultAsync(ct);

        if (pivotLoan is null) return null;

        // Načti všechny záznamy ve stejné grupě
        var items = await _db.Loans
            .Where(l => l.MemberId == pivotLoan.MemberId && l.LoanDate == pivotLoan.LoanDate)
            .OrderBy(l => l.Id)
            .ToListAsync(ct);

        if (items.Count == 0) return null;

        // Načti inventární čísla a názvy přes Equipment + Asset (TPC)
        var itemIds = items.Select(l => l.InventoryItemId).ToList();

        var equipmentMap = await _db.Equipment
            .Where(e => itemIds.Contains(e.Id))
            .Select(e => new { e.Id, e.InventoryNumber, e.Name, CategoryName = e.Category.Name })
            .ToListAsync(ct);

        var assetMap = await _db.Assets
            .Where(a => itemIds.Contains(a.Id))
            .Select(a => new { a.Id, a.InventoryNumber, a.Name, CategoryName = a.Category.Name })
            .ToListAsync(ct);

        var itemInfoMap = equipmentMap
            .Concat(assetMap)
            .ToDictionary(x => x.Id, x => new { x.InventoryNumber, x.Name, x.CategoryName });

        var memberName = await _db.Loans
            .Where(l => l.Id == groupId)
            .Select(l => l.Member.DisplayName ?? l.Member.UserName ?? "")
            .FirstOrDefaultAsync(ct) ?? "";

        var returnedCount = items.Count(l => l.ReturnedDate != null || l.IsClosed);
        var itemCount = items.Count;
        var status = returnedCount == 0 ? "Aktivní"
            : returnedCount == itemCount ? "Uzavřeno"
            : "Částečně vráceno";

        return new LoanDetail
        {
            GroupId = groupId,
            LoanNumber = $"V-{groupId:D5}",
            MemberName = memberName,
            LoanDate = pivotLoan.LoanDate,
            ExpectedReturnDate = pivotLoan.ExpectedReturnDate,
            Status = status,
            Items = items.Select(l =>
            {
                itemInfoMap.TryGetValue(l.InventoryItemId, out var info);
                return new LoanDetailItem
                {
                    LoanId = l.Id,
                    InventoryNumber = info?.InventoryNumber ?? l.InventoryItemId.ToString(),
                    ItemName = info?.Name ?? "—",
                    CategoryName = info?.CategoryName ?? "—",
                    IsReturned = l.ReturnedDate != null || l.IsClosed,
                    ReturnedDate = l.ReturnedDate,
                };
            }).ToList(),
        };
    }

    public async Task<List<MemberSelectItem>> GetActiveMembersAsync(CancellationToken ct = default)
    {
        return await _db.Users
            .OrderBy(u => u.DisplayName ?? u.UserName)
            .Select(u => new MemberSelectItem
            {
                Id = u.Id,
                DisplayName = u.DisplayName ?? u.UserName ?? u.Email ?? u.Id.ToString(),
            })
            .ToListAsync(ct);
    }

    public async Task<InventoryItemLookup> LookupItemAsync(string inventoryNumber, CancellationToken ct = default)
    {
        // Hledat v Equipment
        var eq = await _db.Equipment
            .Where(e => e.InventoryNumber == inventoryNumber)
            .Select(e => new
            {
                e.Id, e.InventoryNumber, e.Name, e.ItemStatus, e.IsActive,
                CategoryName = e.Category.Name,
                LocationName = e.CurrentLocation != null ? e.CurrentLocation.Name : "",
            })
            .FirstOrDefaultAsync(ct);

        int id, itemStatus;
        bool isActive;
        string name, categoryName, locationName, invNumber;

        if (eq != null)
        {
            id = eq.Id; name = eq.Name; itemStatus = eq.ItemStatus; isActive = eq.IsActive;
            categoryName = eq.CategoryName; locationName = eq.LocationName; invNumber = eq.InventoryNumber;
        }
        else
        {
            // Hledat v Asset
            var asset = await _db.Assets
                .Where(a => a.InventoryNumber == inventoryNumber)
                .Select(a => new
                {
                    a.Id, a.InventoryNumber, a.Name, a.ItemStatus, a.IsActive,
                    CategoryName = a.Category.Name,
                    LocationName = a.CurrentLocation != null ? a.CurrentLocation.Name : "",
                })
                .FirstOrDefaultAsync(ct);

            if (asset is null)
            {
                return new InventoryItemLookup
                {
                    Found = false,
                    IsAvailable = false,
                    ErrorMessage = $"Položka s inventárním číslem '{inventoryNumber}' nebyla nalezena.",
                };
            }

            id = asset.Id; name = asset.Name; itemStatus = asset.ItemStatus; isActive = asset.IsActive;
            categoryName = asset.CategoryName; locationName = asset.LocationName; invNumber = asset.InventoryNumber;
        }

        if (!isActive
            || itemStatus == (int)EItemStatus.Disposed
            || itemStatus == (int)EItemStatus.Lost)
        {
            return new InventoryItemLookup
            {
                Found = true,
                IsAvailable = false,
                ErrorMessage = "Položka je vyřazena nebo ztracena.",
                InventoryItemId = id,
                InventoryNumber = invNumber,
                Name = name,
                CategoryName = categoryName,
                CurrentLocationName = locationName,
            };
        }

        if (itemStatus == (int)EItemStatus.Borrowed)
        {
            return new InventoryItemLookup
            {
                Found = true,
                IsAvailable = false,
                ErrorMessage = "Položka je již vypůjčena.",
                InventoryItemId = id,
                InventoryNumber = invNumber,
                Name = name,
                CategoryName = categoryName,
                CurrentLocationName = locationName,
            };
        }

        return new InventoryItemLookup
        {
            Found = true,
            IsAvailable = true,
            InventoryItemId = id,
            InventoryNumber = invNumber,
            Name = name,
            CategoryName = categoryName,
            CurrentLocationName = locationName,
        };
    }

    public async Task<int> CreateLoanAsync(CreateLoan model, CancellationToken ct = default)
    {
        var member = await _db.Users.FindAsync([model.MemberId!.Value], ct)
            ?? throw new InvalidOperationException("Člen nebyl nalezen.");

        var loanDate = DateOnly.FromDateTime(DateTime.Today);
        var createdIds = new List<int>();

        foreach (var invNumber in model.InventoryNumbers)
        {
            var lookup = await LookupItemAsync(invNumber, ct);
            if (!lookup.Found || !lookup.IsAvailable)
                throw new InvalidOperationException(
                    lookup.ErrorMessage ?? $"Položka '{invNumber}' není dostupná.");

            var loan = new Loan
            {
                InventoryItemId = lookup.InventoryItemId,
                MemberId = member.Id,
                LoanDate = loanDate,
            };
            _db.Loans.Add(loan);

            var transaction = new InventoryTransaction
            {
                InventoryItemId = lookup.InventoryItemId,
                TransactionTypeId = (int)ETransactionType.Loan,
                TransactionDate = DateTime.UtcNow,
                Quantity = 1,
            };
            _db.InventoryTransactions.Add(transaction);

            // Aktualizuj stav položky
            await SetItemStatusAsync(lookup.InventoryItemId, (int)EItemStatus.Borrowed, ct);
        }

        await _db.SaveChangesAsync(ct);

        // Načti min Id ze skupiny (záznamy přidané v této transakci)
        var groupId = await _db.Loans
            .Where(l => l.MemberId == member.Id && l.LoanDate == loanDate)
            .MinAsync(l => l.Id, ct);

        return groupId;
    }

    public async Task ReturnItemAsync(int loanId, CancellationToken ct = default)
    {
        var loan = await _db.Loans.FindAsync([loanId], ct)
            ?? throw new InvalidOperationException($"Výpůjčka s ID {loanId} nebyla nalezena.");

        loan.ReturnedDate = DateOnly.FromDateTime(DateTime.Today);
        loan.IsClosed = true;

        var transaction = new InventoryTransaction
        {
            InventoryItemId = loan.InventoryItemId,
            TransactionTypeId = (int)ETransactionType.Return,
            TransactionDate = DateTime.UtcNow,
            Quantity = 1,
        };
        _db.InventoryTransactions.Add(transaction);

        await SetItemStatusAsync(loan.InventoryItemId, (int)EItemStatus.InStock, ct);

        await _db.SaveChangesAsync(ct);
    }

    public async Task ReturnAllAsync(int groupId, CancellationToken ct = default)
    {
        var pivotLoan = await _db.Loans
            .Where(l => l.Id == groupId)
            .Select(l => new { l.MemberId, l.LoanDate })
            .FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException($"Výpůjčka {groupId} nebyla nalezena.");

        var unreturned = await _db.Loans
            .Where(l => l.MemberId == pivotLoan.MemberId
                && l.LoanDate == pivotLoan.LoanDate
                && l.ReturnedDate == null
                && !l.IsClosed)
            .ToListAsync(ct);

        foreach (var loan in unreturned)
        {
            loan.ReturnedDate = DateOnly.FromDateTime(DateTime.Today);
            loan.IsClosed = true;

            _db.InventoryTransactions.Add(new InventoryTransaction
            {
                InventoryItemId = loan.InventoryItemId,
                TransactionTypeId = (int)ETransactionType.Return,
                TransactionDate = DateTime.UtcNow,
                Quantity = 1,
            });

            await SetItemStatusAsync(loan.InventoryItemId, (int)EItemStatus.InStock, ct);
        }

        await _db.SaveChangesAsync(ct);
    }

    private async Task SetItemStatusAsync(int inventoryItemId, int status, CancellationToken ct)
    {
        var eq = await _db.Equipment.FindAsync([inventoryItemId], ct);
        if (eq is not null)
        {
            eq.ItemStatus = status;
            return;
        }

        var asset = await _db.Assets.FindAsync([inventoryItemId], ct);
        if (asset is not null)
            asset.ItemStatus = status;
    }
}
