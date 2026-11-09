using Blogy.Business.Dtos.SocialDtos;
using FluentValidation;

namespace Blogy.Business.Validators.SocialValidators;

public class CreateSocialValidator : AbstractValidator<CreateSocialDto>
{
    public CreateSocialValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Platform adı boş geçilemez.")
            .MinimumLength(2).WithMessage("Platform adı en az 2 karakter olmalıdır.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Yönlendirme linki boş geçilemez.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Lütfen geçerli bir internet adresi giriniz (Örn: https://instagram.com/kullanici).");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("İkon sınıfı boş geçilemez.");
    }
}
