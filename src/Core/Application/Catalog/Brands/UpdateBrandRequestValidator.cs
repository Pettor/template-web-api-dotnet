using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Application.Common.Validation;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Brands;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IRepository<Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                await repository.GetBySpecAsync(new BrandByNameSpec(name), ct)
                    is not Brand existingBrand || existingBrand.Id == brand.Id)
            .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}