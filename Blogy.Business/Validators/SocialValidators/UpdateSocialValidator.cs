using Blogy.Business.Dtos.SocialDtos;
using FluentValidation;

namespace Blogy.Business.Validators.SocialValidators;

public class UpdateSocialValidator : AbstractValidator<UpdateSocialDto>
{
    public UpdateSocialValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçersiz sosyal medya kimliği (Id).");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Platform adı boş geçilemez.")
            .MinimumLength(2).WithMessage("Platform adı en az 2 karakter olmalıdır.");
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Yönlendirme linki boş geçilemez.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Lütfen geçerli bir internet adresi giriniz (Örn: https://...).");
        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("İkon sınıfı boş geçilemez.");
    }
}
