using FluentValidation;

namespace CleanArchitecture.Application.Features.Directors.Commands.CreateDirector
{
	public class CreateDirectorCommandValidator : AbstractValidator<CreateDirectorCommand>
	{
		public CreateDirectorCommandValidator()
		{
			RuleFor(p => p.Nombre).NotNull().WithMessage("{Nombre} no puede ser nulo");
			RuleFor(p => p.Apellido).NotNull().WithMessage("{Apellido} no puede ser nulo");
		}
	}
}
