using Backend.Application.Common.Validation;

namespace Backend.Application.Common.FileStorage;

public class FileUploadRequestValidator : CustomValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Image Name cannot be empty!")
            .MaximumLength(150);

        RuleFor(p => p.Extension)
            .NotEmpty()
            .WithMessage("Image Extension cannot be empty!")
            .MaximumLength(5);

        RuleFor(p => p.Data).NotEmpty().WithMessage("Image Data cannot be empty!");
    }
}
