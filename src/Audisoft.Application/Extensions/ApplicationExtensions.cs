using Audisoft.Application.Contracts.Services;
using Audisoft.Application.Services;
using Audisoft.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Audisoft.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IProfesorService, ProfesorService>();
        services.AddScoped<INotaService, NotaService>();

        services.AddValidatorsFromAssemblyContaining<CreateEstudianteDtoValidator>();

        return services;
    }
}