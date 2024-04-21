//!=>Referencia [1] [Sección 01, 015. Migration con nuevas relaciones]

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
	public class Director : BaseDomainModel
	{
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        //![1] SE AGREGA UNA REFERENCIA HACIA LA CLASE VÍDEO
        public int VideoId { get; set; }
        //![1] SE AGREGA LA CLASE DE SOPORTE HACIA Video
        public virtual Video? Video { get; set; }
    }
}
