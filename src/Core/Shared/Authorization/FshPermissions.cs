using System.Collections.ObjectModel;

namespace Backend.Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllClaims =
    {
        new("View Dashboard", FshAction.View, FshResource.Dashboard),
        new("View Hangfire", FshAction.View, FshResource.Hangfire),
        new("View Users", FshAction.View, FshResource.Users),
        new("Search Users", FshAction.Search, FshResource.Users),
        new("Create Users", FshAction.Create, FshResource.Users),
        new("Update Users", FshAction.Update, FshResource.Users),
        new("Delete Users", FshAction.Delete, FshResource.Users),
        new("Export Users", FshAction.Export, FshResource.Users),
        new("View UserRoles", FshAction.View, FshResource.UserRoles),
        new("Update UserRoles", FshAction.Update, FshResource.UserRoles),
        new("View Roles", FshAction.View, FshResource.Roles),
        new("Create Roles", FshAction.Create, FshResource.Roles),
        new("Update Roles", FshAction.Update, FshResource.Roles),
        new("Delete Roles", FshAction.Delete, FshResource.Roles),
        new("View RoleClaims", FshAction.View, FshResource.RoleClaims),
        new("Update RoleClaims", FshAction.Update, FshResource.RoleClaims),
        new("View Products", FshAction.View, FshResource.Products, IsBasic: true),
        new("Search Products", FshAction.Search, FshResource.Products, IsBasic: true),
        new("Create Products", FshAction.Create, FshResource.Products),
        new("Update Products", FshAction.Update, FshResource.Products),
        new("Delete Products", FshAction.Delete, FshResource.Products),
        new("Export Products", FshAction.Export, FshResource.Products),
        new("View Brands", FshAction.View, FshResource.Brands, IsBasic: true),
        new("Search Brands", FshAction.Search, FshResource.Brands, IsBasic: true),
        new("Create Brands", FshAction.Create, FshResource.Brands),
        new("Update Brands", FshAction.Update, FshResource.Brands),
        new("Delete Brands", FshAction.Delete, FshResource.Brands),
        new("Generate Brands", FshAction.Generate, FshResource.Brands),
        new("Clean Brands", FshAction.Clean, FshResource.Brands),
        new("View Tenants", FshAction.View, FshResource.Tenants, IsRoot: true),
        new("Create Tenants", FshAction.Create, FshResource.Tenants, IsRoot: true),
        new("Update Tenants", FshAction.Update, FshResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FshAction.UpgradeSubscription, FshResource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllClaims);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllClaims.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllClaims.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllClaims.Where(p => p.IsBasic).ToArray());
}