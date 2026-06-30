using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace SportSys.Database.Enums;

public static class EnumExtensions
{
    /// <summary>
    /// Vrátí lokalizovaný popisek enum hodnoty z RESX (dle Thread.CurrentUICulture).
    /// Pokud [Display] atribut chybí, vrátí <c>value.ToString()</c>.
    /// </summary>
    public static string GetDisplayName(this Enum value)
    {
        var attr = value.GetType()
                        .GetField(value.ToString())
                        ?.GetCustomAttribute<DisplayAttribute>();

        if (attr == null) return value.ToString();

        // Použij ResourceManager přímo — vyhne se LocalizableString, která vyžaduje
        // public static string properties na resource třídě (jako generovaný Designer.cs).
        if (attr.ResourceType != null && attr.Name != null)
        {
            var rm = attr.ResourceType
                         .GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.Static)
                         ?.GetValue(null) as ResourceManager;
            if (rm != null)
                return rm.GetString(attr.Name) ?? attr.Name;
        }

        return attr.Name ?? value.ToString();
    }
}
