using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
	// Haz la validación sobre el UpdateStreamerCommand
	public class UpdateStreamerCommandValidator : AbstractValidator<UpdateStreamerCommand>
	{
		public UpdateStreamerCommandValidator()
		{
			RuleFor(p => p.Nombre)
				.NotNull().WithMessage("{Nombre} no permite valores nulos");
			RuleFor(p => p.Url)
				.NotNull().WithMessage("{Url} no permite valores nulos");
		}
	}
}
