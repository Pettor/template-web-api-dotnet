using Backend.Application.Common.Persistence;
using Backend.Application.Common.Validation;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepositoryBase<Brand> repository, IStringLocalizer localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}
