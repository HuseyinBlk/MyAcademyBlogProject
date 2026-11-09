using System.Reflection;
using Blogy.Business.Configurations;
using Blogy.Business.Mappings;
using Blogy.Business.Validators.CategoryValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Blogy.Business.Extensions;

public static class ServiceRegistrations
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services,IConfiguration configuration)
    {

        services.Scan(opt =>
        {
            opt.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                .UsingRegistrationStrategy(registrationStrategy: RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        services.Configure<GeminiOptions>(configuration.GetSection(GeminiOptions.Section));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Section));
        services.Configure<HuggingFaceOptions>(configuration.GetSection(HuggingFaceOptions.Section));

        services.AddAutoMapper(typeof(CategoryMappings).Assembly);
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);

        return services;
    }
}
