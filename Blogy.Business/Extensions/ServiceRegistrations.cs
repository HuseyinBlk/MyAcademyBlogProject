using System.Reflection;
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
using Scrutor;

namespace Blogy.Business.Extensions;

public static class ServiceRegistrations
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        
        services.Scan(opt =>
        {
            opt.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                .UsingRegistrationStrategy(registrationStrategy: RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
        
        services.AddAutoMapper(typeof(CategoryMappings).Assembly);
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);
        
        return services;
    }
}
