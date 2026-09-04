using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Validators;

public class CreateNotaDtoValidator : AbstractValidator<CreateNotaRequest>
{
    public CreateNotaDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Valor)
            .InclusiveBetween(0, 20);

        RuleFor(x => x.Fecha)
            .NotEmpty();

        RuleFor(x => x.Materia)
            .IsInEnum();

        RuleFor(x => x.EstudianteId)
            .GreaterThan(0);

        RuleFor(x => x.ProfesorId)
            .GreaterThan(0);
    }
}

public class UpdateNotaDtoValidator : AbstractValidator<UpdateNotaRequest>
{
    public UpdateNotaDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Valor)
            .InclusiveBetween(0, 20);

        RuleFor(x => x.Fecha)
            .NotEmpty();

        RuleFor(x => x.Materia)
            .IsInEnum();

        RuleFor(x => x.EstudianteId)
            .GreaterThan(0);

        RuleFor(x => x.ProfesorId)
            .GreaterThan(0);
    }
}