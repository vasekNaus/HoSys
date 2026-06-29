using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models.inventory;

/// <summary>Řádek v přehledu výpůjček (skupina dle MemberId + LoanDate).</summary>
public class LoanListItem
{
    /// <summary>min(Loan.Id) v grupě – slouží jako GroupId.</summary>
    public int GroupId { get; set; }
    /// <summary>Formát: V-{GroupId:D5}</summary>
    public string LoanNumber { get; set; } = "";
    public string MemberName { get; set; } = "";
    public DateOnly LoanDate { get; set; }
    public int ItemCount { get; set; }
    public int ReturnedCount { get; set; }
    /// <summary>Aktivní / Částečně vráceno / Uzavřeno</summary>
    public string Status { get; set; } = "";
}

/// <summary>Hlavička detailu výpůjčky (jen čtení).</summary>
public class LoanDetail
{
    public int GroupId { get; set; }
    public string LoanNumber { get; set; } = "";
    public string MemberName { get; set; } = "";
    public DateOnly LoanDate { get; set; }
    public DateOnly? ExpectedReturnDate { get; set; }
    public string Status { get; set; } = "";
    public List<LoanDetailItem> Items { get; set; } = [];
}

/// <summary>Řádek tabulky položek ve výpůjčce.</summary>
public class LoanDetailItem
{
    /// <summary>Id záznamu Loan.</summary>
    public int LoanId { get; set; }
    public string InventoryNumber { get; set; } = "";
    public string ItemName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public bool IsReturned { get; set; }
    public DateOnly? ReturnedDate { get; set; }
}

/// <summary>Vstup pro vytvoření výpůjčky.</summary>
public class CreateLoan
{
    [Required(ErrorMessage = "Vyberte člena.")]
    [Display(Name = "Člen")]
    public int? MemberId { get; set; }

    public List<string> InventoryNumbers { get; set; } = [];
}

/// <summary>Výsledek vyhledání položky dle inventárního čísla (QR skener / AJAX lookup).</summary>
public class InventoryItemLookup
{
    public bool Found { get; set; }
    public bool IsAvailable { get; set; }
    public string? ErrorMessage { get; set; }
    public int InventoryItemId { get; set; }
    public string InventoryNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string CurrentLocationName { get; set; } = "";
}

/// <summary>Položka selectu pro výběr člena ve formuláři výpůjčky.</summary>
public class MemberSelectItem
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = "";
}

/// <summary>Filtr na přehledu výpůjček.</summary>
public class LoanFilter
{
    [Display(Name = "Člen")]
    public string? MemberName { get; set; }

    [Display(Name = "Pouze aktivní")]
    public bool ActiveOnly { get; set; }

    [Display(Name = "Datum od")]
    public DateOnly? DateFrom { get; set; }

    [Display(Name = "Datum do")]
    public DateOnly? DateTo { get; set; }
}
