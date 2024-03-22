using Backend.Application.Common.Persistence;
using Backend.Application.Common.Specification;
using Backend.Application.Identity.Roles;
using Backend.Application.Identity.Users;
using Backend.Domain.Catalog;

namespace Backend.Application.Dashboard;

public class GetStatsRequestHandler(
    IUserService userService,
    IRoleService roleService,
    IReadRepository<Brand> brandRepo,
    IReadRepository<Product> productRepo,
    IStringLocalizer<GetStatsRequestHandler> localizer)
    : IRequestHandler<GetStatsRequest, StatsDto>
{
    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            ProductCount = await productRepo.CountAsync(cancellationToken),
            BrandCount = await brandRepo.CountAsync(cancellationToken),
            UserCount = await userService.GetCountAsync(cancellationToken),
            RoleCount = await roleService.GetCountAsync(cancellationToken)
        };

        var selectedYear = DateTime.Now.Year;
        var productsFigure = new double[13];
        var brandsFigure = new double[13];
        for (var i = 1; i <= 12; i++)
        {
            var month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01);
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

            var brandSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Brand>(filterStartDate, filterEndDate);
            var productSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Product>(filterStartDate, filterEndDate);

            brandsFigure[i - 1] = await brandRepo.CountAsync(brandSpec, cancellationToken);
            productsFigure[i - 1] = await productRepo.CountAsync(productSpec, cancellationToken);
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = localizer["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = localizer["Brands"], Data = brandsFigure });

        return stats;
    }
}
