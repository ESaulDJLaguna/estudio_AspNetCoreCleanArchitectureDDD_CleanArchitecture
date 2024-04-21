//!=>Referencia [1] [Sección 01, 014. Muchos a muchos en EF]

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
	public class Actor : BaseDomainModel
	{
        public Actor()
        {
            //![1] DEBEMOS INICIALIZAR LA RELACIÓN MUCHOS A MUCHOS
            Videos = new HashSet<Video>();
        }

        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        //![1] SE CREA LA RELACIÓN MUCHOS A MUCHOS CON VIDEOS
        public virtual ICollection<Video> Videos { get; set; }
    }
}
