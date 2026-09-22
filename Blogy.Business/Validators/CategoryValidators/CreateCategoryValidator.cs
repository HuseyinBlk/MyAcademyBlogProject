using Blogy.Business.Dtos.CategoryDtos;
using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Validators.CategoryValidators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public int MinLength = 3;
    public int MaxLength = 20;
    
    public CreateCategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Kategori Adı Boş Bırakılamaz.")
            .MinimumLength(MinLength).WithMessage("Kategori Adı En Az " + MinLength +"Karakter Olmalı.")
            .MaximumLength(MaxLength).WithMessage("Kategori Adı En Fazla "+ MaxLength +" Karakter Olmalı.");
    }
}