using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Validators;

public class CreateEstudianteDtoValidator : AbstractValidator<CreateEstudianteRequest>
{
    public CreateEstudianteDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class UpdateEstudianteDtoValidator : AbstractValidator<UpdateEstudianteRequest>
{
    public UpdateEstudianteDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);
    }
}