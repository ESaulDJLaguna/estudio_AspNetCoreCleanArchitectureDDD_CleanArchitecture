namespace LeerData.Models
{
	public class Book
	{
		public int BookId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime PublishDate { get; set; }

		/*
         * La idea de crear esta propiedad, es que si instanciamos a Book, también podemos obtener valores de Price a través de Book
         */
		public Price PromotionPrice { get; set; }
		public HashSet<Comment> ListComment { get; set; }
		/*
         * Book tendrá una referencia hacia AuthorBook de 1 a muchos. Esto significa que un registro de la entidad Book va a tener MUCHOS AuthorBook.
         * 
         * Se hace esto porque AuthorBook es la que nos va a llevar posteriormente a la entidad Author
         */
		public HashSet<AuthorBook> AuthorLink { get; set; }
	}
}