//!=>Referencia [1] [Sección 01, 004. Creación de Modelo]
//!=>Referencia [2] [Sección 01, 013. Fluent API]
//!=>Referencia [3] [Sección 01, 014. Muchos a muchos en EF]

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
	public class Streamer : BaseDomainModel
	{
		//! [3] HEREDARÁ Id DE LA CLASE BaseDomainModel
		//[3] public int Id { get; set; }
		//![1] LA VERSIÓN DE .NET 6 NOS OBLIGA A SETEAR UN VALOR POR DEFECTO PARA LAS PROPIEDADES
		//[1] public string Nombre { get; set; } = string.Empty;
		public string? Nombre { get; set; }
		//![1] OPCIONALMENTE PODEMOS INDICARLE QUE ACEPTE NULOS
		public string? Url { get; set; }
		//![2] UN Streamer PUEDE TENER MUCHOS Videos, POR ESO SE REQUIERE DE UNA PROPIEDAD QUE REPRESENTE DICHA RELACIÓN.
		// Le indicamos con ? que sea opcionalmente nulable
        public ICollection<Video>? Videos { get; set; }
    }
}
