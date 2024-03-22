using Backend.Application.Catalog.Brands.Specifications;
using Backend.Application.Common.Persistence;
using Backend.Application.Common.Validation;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Queries.Update;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IRepository<Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct)
                    is not Brand existingBrand || existingBrand.Id == brand.Id)
            .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}
