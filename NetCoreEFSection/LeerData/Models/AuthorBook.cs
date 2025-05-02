namespace LeerData.Models
{
	/*
	 * En esta clase se encontrarán las "anclas" para Author y Book. Mientras que las colecciones de "muchos" estarán dentro de Author y Book.
	 * 
	 * Podemos observar que en esta entidad tenemos las propiedades BookId y AuthorId. Estas dos propiedades (en conjunto) son llaves primarias y llaves foráneas al mismo tiempo (cada propiedad es una llave foránea). Esto es así porque a EntityFramework Core se le tiene que indicar que NO tiene una llave primaria sino que tiene DOS llaves primarias, ya que si no se hace eso, va a arrojar errores. Esto se configura desde AppBookSaleContext
	 */
	public class AuthorBook
	{
		public int BookId { get; set; }
		/*
		 * Se creará el "ancla" hacia Book. Es decir, se creará una propiedad que será una representación de la clase Book dentro de AuthorBook.
		 * 
		 * Esta es el "ancla" que va a enlazar la clase AuthorBook con la clase Book y a su vez Book tiene  una colección que me va a enlazar con AuthorBook
		 */
		public Book Book { get; set; }
		public int AuthorId { get; set; }
		/*
		 * Se creará el ancla que será la referencia a la clase Author
		 */
		public Author Author { get; set; }
	}
}
