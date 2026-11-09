using Blogy.Business.Dtos.AboutDtos;
using FluentValidation;

namespace Blogy.Business.Validators.AboutValidators;

public class CreateAboutValidators : AbstractValidator<CreateAboutDto>
{
    public CreateAboutValidators()
    {
        RuleFor(x => x.Title1)
            .NotEmpty().WithMessage("1. Bölüm başlığı boş geçilemez.")
            .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir.");

        RuleFor(x => x.Description1)
            .NotEmpty().WithMessage("1. Bölüm açıklama metni boş geçilemez.");

        RuleFor(x => x.Title2)
            .NotEmpty().WithMessage("2. Bölüm başlığı boş geçilemez.")
            .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir.");

        RuleFor(x => x.Description2)
            .NotEmpty().WithMessage("2. Bölüm açıklama metni boş geçilemez.");

        RuleFor(x => x.FeatureTitle1)
            .NotEmpty().WithMessage("1. Özellik başlığı boş geçilemez.");

        RuleFor(x => x.FeatureTitle2)
            .NotEmpty().WithMessage("2. Özellik başlığı boş geçilemez.");

        RuleFor(x => x.FeatureTitle3)
            .NotEmpty().WithMessage("3. Özellik başlığı boş geçilemez.");
    }
}
