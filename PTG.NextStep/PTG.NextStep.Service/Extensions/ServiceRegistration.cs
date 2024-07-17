using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PTG.NextStep.Domain;
using PTG.NextStep.Service.Validators;
using System.Reflection;

namespace PTG.NextStep.Service.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidationDictionary, ValidationDictionary>();
            return services;
        }
    }
}
