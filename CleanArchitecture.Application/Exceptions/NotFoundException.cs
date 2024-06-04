//!=>Referencia [1] [Sección 08, 040. Update Command]

namespace CleanArchitecture.Application.Exceptions
{
	public class NotFoundException : ApplicationException
	{
		//! [1] name REPRESENTA LA CLASE QUE DISPARA EL ERROR, key ES EL ID DEL REGISTRO QUE PROVOCÓ EL ERROR
		public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) no fue encontrado")
        {
        }
    }
}
