//!=>Referencia [1] [Sección 01, 004. Creación de Modelo]
//!=>Referencia [2] [Sección 01, 013. Fluent API]
//!=>Referencia [3] [Sección 01, 014. Muchos a muchos en EF]

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
	public class Video : BaseDomainModel
	{
        public Video()
        {
			//![1] DEBEMOS INICIALIZAR LA RELACIÓN MUCHOS A MUCHOS
			Actores = new HashSet<Actor>();
        }
        //! [3] HEREDARÁ Id DE LA CLASE BaseDomainModel
        //[3] public int Id { get; set; }
        public string? Nombre { get; set; }
		//! [1] NECESITAMOS CREAR UNA COLUMNA QUE REPRESENTA EN LA BASE DE DATOS A STREAMER (LA LLAVE FORÁNEA)
		//! [2] POR NOMENCLATURA, EFC ENTIENE QUE ES UNA LLAVE FORÁNEA SI SU NOMBRE INICIA CON EL MISMO NOMBRE DE COMO SE LLAMA LA CLASE RELACIONADA (Streamer) Y DESPUÉS SE LE AGREGA Id
		// Un vídeo solo puede pertenecer a un Streamer
		public int StreamerId{ get; set; }
		//! [1] TAMBIÉN SE NECESITA UN ANCLA (UNA CLASE QUE REPRESENTE A LA ENTIDAD). CUANDO SE LE COLOCA VIRTUAL A UNA PROPIEDAD O A UN MÉTODO, SE ESTÁ INDICANDO QUE PUEDE SER SOBREESCRITA POR UNA CLASE DERIVADA EN EL FUTURO
		public virtual Streamer? Streamer { get; set; }
        //![3] SE CREA LA RELACIÓN MUCHOS A MUCHOS CON ACTORES
        public virtual ICollection<Actor> Actores { get; set; }
        public virtual Director? Director { get; set; }
    }
}
