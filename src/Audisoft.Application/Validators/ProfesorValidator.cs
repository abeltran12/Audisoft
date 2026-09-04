using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Validators;

public class CreateProfesorDtoValidator : AbstractValidator<CreateProfesorRequest>
{
    public CreateProfesorDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class UpdateProfesorDtoValidator : AbstractValidator<UpdateProfesorRequest>
{
    public UpdateProfesorDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);
    }
}