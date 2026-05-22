using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
        return attr?.GetName() ?? value.ToString();
    }
}
