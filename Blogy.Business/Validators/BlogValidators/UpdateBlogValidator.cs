using Blogy.Business.Dtos.BlogDtos;
using FluentValidation;

namespace Blogy.Business.Validators.BlogValidators;

public class UpdateBlogValidator : AbstractValidator<UpdateBlogDto>
{
    private const int TitleMinLength = 3;
    private const int TitleMaxLength = 150;
    private const int DescriptionMinLength = 20;
    private const int DescriptionMaxLength = 5000;
    private const int ImageMaxLength = 500;
    
    public UpdateBlogValidator()
    {
        RuleFor(blog => blog.Id)
            .GreaterThan(0)
            .WithMessage("Güncellenecek blog bilgisi geçersiz.");

        RuleFor(blog => blog.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Blog başlığı boş bırakılamaz.")
            .MinimumLength(TitleMinLength)
            .WithMessage(
                $"Blog başlığı en az {TitleMinLength} karakter olmalıdır.")
            .MaximumLength(TitleMaxLength)
            .WithMessage(
                $"Blog başlığı en fazla {TitleMaxLength} karakter olmalıdır.");

        RuleFor(blog => blog.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Blog açıklaması boş bırakılamaz.")
            .MinimumLength(DescriptionMinLength)
            .WithMessage(
                $"Blog açıklaması en az {DescriptionMinLength} karakter olmalıdır.")
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(
                $"Blog açıklaması en fazla {DescriptionMaxLength} karakter olmalıdır.");

        RuleFor(blog => blog.CoverImage)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Kapak görseli boş bırakılamaz.")
            .MaximumLength(ImageMaxLength)
            .WithMessage(
                $"Kapak görseli adresi en fazla {ImageMaxLength} karakter olmalıdır.");

        RuleFor(blog => blog.BlogImage)
            .MaximumLength(ImageMaxLength)
            .WithMessage(
                $"Blog görseli adresi en fazla {ImageMaxLength} karakter olmalıdır.");

        RuleFor(blog => blog.BlogImage2)
            .MaximumLength(ImageMaxLength)
            .WithMessage(
                $"İkinci blog görseli adresi en fazla {ImageMaxLength} karakter olmalıdır.");

        RuleFor(blog => blog.CategoryId)
            .GreaterThan(0)
            .WithMessage("Bir kategori seçmelisiniz.");
    }
}