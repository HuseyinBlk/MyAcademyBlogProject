using Blogy.Business.Dtos.ContactDtos;
using FluentValidation;

namespace Blogy.Business.Validators.ContactValidators;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad ve soyad alanı boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz (Örn: ornek@email.com).");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Mesaj konusu boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalıdır.")
            .MaximumLength(150).WithMessage("Konu en fazla 150 karakter olabilir.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj içeriği boş bırakılamaz.")
            .MinimumLength(10).WithMessage("Mesajınız en az 10 karakter uzunluğunda olmalıdır.")
            .MaximumLength(2000).WithMessage("Mesajınız en fazla 2000 karakter olabilir.");
    }
}
