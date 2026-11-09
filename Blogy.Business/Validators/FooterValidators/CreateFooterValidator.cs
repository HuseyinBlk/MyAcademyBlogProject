using Blogy.Business.Dtos.FooterDtos;
using FluentValidation;

namespace Blogy.Business.Validators.FooterValidators;

public class CreateFooterValidator : AbstractValidator<CreateFooterDto>
{
    public CreateFooterValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Footer açıklama metni boş geçilemez.")
            .MinimumLength(10).WithMessage("Footer metni en az 10 karakter olmalıdır.");
    }
}
