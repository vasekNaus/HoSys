using Microsoft.EntityFrameworkCore;
using SportSys.Contract.Models.inventory;
using SportSys.Database.Context;
using SportSys.Database.Enums;
using SportSys.Database.Models.inventory;
using DbAsset = SportSys.Database.Models.inventory.Asset;
using DbEquipment = SportSys.Database.Models.inventory.Equipment;

namespace SportSys.Contract.Services;

public class InventoryItemService
{
    private readonly SportSysDbContext _db;

    public InventoryItemService(SportSysDbContext db)
    {
        _db = db;
    }

    public async Task<List<InventoryItemListItem>> GetListAsync(InventoryItemFilter filter, CancellationToken ct = default)
    {
        var result = new List<InventoryItemListItem>();

        bool includeEquipment = filter.ItemType is null or "Equipment";
        bool includeAsset = filter.ItemType is null or "Asset";

        if (includeEquipment)
        {
            var items = await ApplyFilter(_db.Equipment.AsQueryable(), filter)
                .Select(e => new InventoryItemListItem
                {
                    Id = e.Id,
                    ItemType = "Equipment",
                    ItemTypeName = "Výstroj",
                    InventoryNumber = e.InventoryNumber,
                    Name = e.Name,
                    CategoryName = e.Category.Name,
                    ManufacturerName = e.Manufacturer != null ? e.Manufacturer.Name : null,
                    ItemStatus = e.ItemStatus,
                    IsActive = e.IsActive,
                })
                .ToListAsync(ct);
            result.AddRange(items);
        }

        if (includeAsset)
        {
            var items = await ApplyFilter(_db.Assets.AsQueryable(), filter)
                .Select(a => new InventoryItemListItem
                {
                    Id = a.Id,
                    ItemType = "Asset",
                    ItemTypeName = "Majetek",
                    InventoryNumber = a.InventoryNumber,
                    Name = a.Name,
                    CategoryName = a.Category.Name,
                    ManufacturerName = a.Manufacturer != null ? a.Manufacturer.Name : null,
                    ItemStatus = a.ItemStatus,
                    IsActive = a.IsActive,
                })
                .ToListAsync(ct);
            result.AddRange(items);
        }

        // Přeložit stav in-memory (EF Core nedokáže přeložit GetDisplayName do SQL)
        foreach (var item in result)
            item.StatusName = ((EItemStatus)item.ItemStatus).GetDisplayName();

        return [.. result.OrderBy(x => x.Name).ThenBy(x => x.InventoryNumber)];
    }

    public async Task<InventoryItemForm?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var eq = await _db.Equipment.Where(e => e.Id == id).FirstOrDefaultAsync(ct);
        if (eq is not null)
            return MapToForm(eq, "Equipment");

        var asset = await _db.Assets.Where(a => a.Id == id).FirstOrDefaultAsync(ct);
        if (asset is not null)
            return MapToForm(asset, "Asset");

        return null;
    }

    public async Task<InventoryItemForm> CreateAsync(InventoryItemForm form, CancellationToken ct = default)
    {
        if (form.ItemType == "Equipment")
        {
            var entity = new DbEquipment
            {
                InventoryNumber = form.InventoryNumber!,
                Name = form.Name!,
                Description = form.Description,
                CategoryId = form.CategoryId,
                ManufacturerId = form.ManufacturerId,
                AssignedLocationId = form.AssignedLocationId,
                CurrentLocationId = form.CurrentLocationId,
                ItemStatus = form.ItemStatus,
                AcquisitionDate = form.AcquisitionDate,
                AcquisitionPrice = form.AcquisitionPrice,
                IsActive = form.IsActive,
                CreatedAt = DateTime.UtcNow,
                Size = form.Size,
            };
            _db.Equipment.Add(entity);
            await _db.SaveChangesAsync(ct);
            form.Id = entity.Id;
        }
        else
        {
            var entity = new DbAsset
            {
                InventoryNumber = form.InventoryNumber!,
                Name = form.Name!,
                Description = form.Description,
                CategoryId = form.CategoryId,
                ManufacturerId = form.ManufacturerId,
                AssignedLocationId = form.AssignedLocationId,
                CurrentLocationId = form.CurrentLocationId,
                ItemStatus = form.ItemStatus,
                AcquisitionDate = form.AcquisitionDate,
                AcquisitionPrice = form.AcquisitionPrice,
                IsActive = form.IsActive,
                CreatedAt = DateTime.UtcNow,
                SerialNumber = form.SerialNumber,
                WarrantyUntil = form.WarrantyUntil,
                ExternalId = form.ExternalId,
            };
            _db.Assets.Add(entity);
            await _db.SaveChangesAsync(ct);
            form.Id = entity.Id;
        }

        return form;
    }

    public async Task UpdateAsync(InventoryItemForm form, CancellationToken ct = default)
    {
        if (form.ItemType == "Equipment")
        {
            var entity = await _db.Equipment.FindAsync([form.Id], ct)
                ?? throw new InvalidOperationException($"Výstroj s ID {form.Id} nebyla nalezena.");

            MapCommon(form, entity);
            entity.Size = form.Size;
            entity.ModifiedAt = DateTime.UtcNow;
        }
        else
        {
            var entity = await _db.Assets.FindAsync([form.Id], ct)
                ?? throw new InvalidOperationException($"Majetek s ID {form.Id} nebyl nalezen.");

            MapCommon(form, entity);
            entity.SerialNumber = form.SerialNumber;
            entity.WarrantyUntil = form.WarrantyUntil;
            entity.ExternalId = form.ExternalId;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(ct);
    }

    // ── Privátní pomocné metody ──────────────────────────────────────────────────

    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, InventoryItemFilter filter)
        where T : InventoryItem
    {
        if (!string.IsNullOrWhiteSpace(filter.NameFilter))
            query = query.Where(x => x.Name.Contains(filter.NameFilter!) || x.InventoryNumber.Contains(filter.NameFilter!));
        if (filter.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
        if (filter.StatusFilter.HasValue)
            query = query.Where(x => x.ItemStatus == filter.StatusFilter.Value);
        if (filter.ActiveOnly)
            query = query.Where(x => x.IsActive);
        return query;
    }

    private static InventoryItemForm MapToForm(DbEquipment e, string itemType) => new()
    {
        Id = e.Id,
        ItemType = itemType,
        InventoryNumber = e.InventoryNumber,
        Name = e.Name,
        Description = e.Description,
        CategoryId = e.CategoryId,
        ManufacturerId = e.ManufacturerId,
        AssignedLocationId = e.AssignedLocationId,
        CurrentLocationId = e.CurrentLocationId,
        ItemStatus = e.ItemStatus,
        AcquisitionDate = e.AcquisitionDate,
        AcquisitionPrice = e.AcquisitionPrice,
        IsActive = e.IsActive,
        Size = e.Size,
    };

    private static InventoryItemForm MapToForm(DbAsset a, string itemType) => new()
    {
        Id = a.Id,
        ItemType = itemType,
        InventoryNumber = a.InventoryNumber,
        Name = a.Name,
        Description = a.Description,
        CategoryId = a.CategoryId,
        ManufacturerId = a.ManufacturerId,
        AssignedLocationId = a.AssignedLocationId,
        CurrentLocationId = a.CurrentLocationId,
        ItemStatus = a.ItemStatus,
        AcquisitionDate = a.AcquisitionDate,
        AcquisitionPrice = a.AcquisitionPrice,
        IsActive = a.IsActive,
        SerialNumber = a.SerialNumber,
        WarrantyUntil = a.WarrantyUntil,
        ExternalId = a.ExternalId,
    };

    private static void MapCommon(InventoryItemForm form, InventoryItem entity)
    {
        entity.InventoryNumber = form.InventoryNumber!;
        entity.Name = form.Name!;
        entity.Description = form.Description;
        entity.CategoryId = form.CategoryId;
        entity.ManufacturerId = form.ManufacturerId;
        entity.AssignedLocationId = form.AssignedLocationId;
        entity.CurrentLocationId = form.CurrentLocationId;
        entity.ItemStatus = form.ItemStatus;
        entity.AcquisitionDate = form.AcquisitionDate;
        entity.AcquisitionPrice = form.AcquisitionPrice;
        entity.IsActive = form.IsActive;
    }
}
