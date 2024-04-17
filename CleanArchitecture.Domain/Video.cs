//!=>Referencia [1] [Sección 01, 004. Creación de Modelo]

namespace CleanArchitecture.Domain
{
	public class Video
	{
		public int Id { get; set; }
		public string? Nombre { get; set; }
		//! [1] NECESITAMOS CREAR UNA COLUMNA QUE REPRESENTA EN LA BASE DE DATOS A STREAMER (LA LLAVE FORÁNEA)
		public int StreamerId{ get; set; }
		//! [1] TAMBIÉN SE NECESITA UN ANCLA (UNA CLASE QUE REPRESENTE A LA ENTIDAD). CUANDO SE LE COLOCA VIRTUAL A UNA PROPIEDAD O A UN MÉTODO, SE ESTÁ INDICANDO QUE PUEDE SER SOBREESCRITA POR UNA CLASE DERIVADA EN EL FUTURO
		public virtual Streamer? Streamer { get; set; }
	}
}
