using Blogy.Business.Dtos.FooterDtos;
using FluentValidation;

namespace Blogy.Business.Validators.FooterValidators;

public class UpdateFooterValidator : AbstractValidator<UpdateFooterDto>
{
    public UpdateFooterValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçersiz kimlik.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Footer açıklama metni boş geçilemez.")
            .MinimumLength(10).WithMessage("Footer metni en az 10 karakter olmalıdır.");
    }
}
