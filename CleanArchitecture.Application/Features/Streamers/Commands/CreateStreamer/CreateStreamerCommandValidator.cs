//!=>Referencia [1] [Sección 08, 038. Validaciones en CQRS]

using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    //! [1] PARA AGREGAR REGLAS DE VALIDACIÓN HEREDA DE AbstractValidator E INDICA LOS PARÁMETROS A VALIDAR (StreamerCommand)
    public class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommand>
    {
        public CreateStreamerCommandValidator()
        {
            RuleFor(p => p.Nombre)
                //! [1] NO PERMITE VALORES VACÍOS, DE SER VACÍO MUESTRA UN MENSAJE PERSONALIZADO
                .NotEmpty().WithMessage("{Nombre} no puede estar en blanco")
                //! [1] NO ACEPTA UN VALOR NULO
                .NotNull()
                //! [1] LONGITUD MÁXIMA ACEPTADA
                .MaximumLength(50).WithMessage("{Nombre} no puede exceder los 50 caracteres");
            RuleFor(p => p.Url)
                .NotEmpty().WithMessage("La {Url} no puede estar en blanco");
        }
    }
}
