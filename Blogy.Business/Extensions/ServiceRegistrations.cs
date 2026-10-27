using Blogy.Business.Mappings;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.FileServices;
using Blogy.Business.Services.UserServices;
using Blogy.Business.Validators.CategoryValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogy.Business.Extensions;

public static class ServiceRegistrations
{
    public static IServiceCollection AddBuinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CategoryMappings).Assembly);
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICommentService,CommentService>();
        
        return services;
    }
}
