using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Validators;

public class CommentValidator : AbstractValidator<Comment>
{
    public CommentValidator()
    {
        RuleFor(c => c.Content)
            .NotEmpty()
            .WithMessage("Yorum içeriği boş olamaz.")
            .MaximumLength(200)
            .WithMessage("Yorum içeriği 200 karakterden uzun olamaz.");
        RuleFor(u => u.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı boş olamaz.");
        RuleFor(b => b.BlogId)
            .NotEmpty()
            .WithMessage("Blog boş olamaz.");
    }
}