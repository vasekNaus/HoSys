namespace SportSys.Contract.Auth;

/// <summary>
/// Konstanty pro claim types používané v business autorizaci.
/// </summary>
public static class PermissionClaimTypes
{
  /// <summary>
  /// Claim type pro business permissions (hodnota = Code oprávnění, např. "invoice.approve").
  /// </summary>
  public const string Permission = "permission";

  /// <summary>
  /// Claim type pro business role (hodnota = Code business role).
  /// </summary>
  public const string BusinessRole = "business_role";
}
