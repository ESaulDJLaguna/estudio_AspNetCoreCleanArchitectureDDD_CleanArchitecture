using FluentValidation.Results;

namespace CleanArchitecture.Application.Exceptions
{
	public class ValidationException : ApplicationException
	{
        public ValidationException() : base("Se presentaron uno o más errores de validación")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
			Errors = failures
                //! En la expresión lambda quiero de devuelva el nombre de la propiedad (la propiedad a evaluar) y que devuelva el mensaje de error a enviar al cliente
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                //! Quiero que estos dos valores llenen al diccionario, el primer failureGroup representa a cada elemento del diccionario y el segundo indica que puede haber un conjunto de validaciones y resultados que pueden estarse disparando
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray()
                );
		}

        public IDictionary<string, string[]> Errors { get; set; }
    }
}
