//!=>Referencia [1] [Sección 01, 004. Creación de Modelo]

namespace CleanArchitecture.Domain
{
	public class Streamer
	{
		public int Id { get; set; }
		//![1] LA VERSIÓN DE .NET 6 NOS OBLIGA A SETEAR UN VALOR POR DEFECTO PARA LAS PROPIEDADES
		//[1] public string Nombre { get; set; } = string.Empty;
		public string? Nombre { get; set; }
		//![1] OPCIONALMENTE PODEMOS INDICARLE QUE ACEPTE NULOS
		public string? Url { get; set; }
	}
}
