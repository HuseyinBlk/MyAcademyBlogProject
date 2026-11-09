using Blogy.Business.Dtos.TagDtos;
using FluentValidation;

namespace Blogy.Business.Validators.TagValidators;

public class CreateTagValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Etiket adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Etiket adı en az 2 karakter olmalıdır.")
            .MaximumLength(30).WithMessage("Etiket adı en fazla 30 karakter olmalıdır.");
    }
}
