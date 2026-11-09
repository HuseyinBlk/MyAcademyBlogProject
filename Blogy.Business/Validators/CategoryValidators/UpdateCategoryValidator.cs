using Blogy.Business.Dtos.CategoryDtos;
using FluentValidation;

namespace Blogy.Business.Validators.CategoryValidators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public int MinLength = 3;
    public int MaxLength = 20;

    public UpdateCategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Kategori Adı Boş Bırakılamaz.")
            .MinimumLength(MinLength).WithMessage("Kategori Adı En Az " + MinLength +"Karakter Olmalı.")
            .MaximumLength(MaxLength).WithMessage("Kategori Adı En Fazla "+ MaxLength +" Karakter Olmalı.");

    }

}
