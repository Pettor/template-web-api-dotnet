using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Application.Common.Validation;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Brands;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepository<Brand> repository, IStringLocalizer<CreateBrandRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new BrandByNameSpec(name), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}