namespace LeerData.Models
{
	public class Price
	{
		public int PriceId { get; set; }
		public decimal CurrentPrice { get; set; }
		public decimal Promotion { get; set; }
		// Llave foránea (Relacion 1:1)
		public int BookId { get; set; }

		/*
         * Las tablas Books y Prices SOLO están enlazadas desde Sql Server por medio de sus llaves foráneas, pero en .NET NO existe esa relación.
         * 
         * Para crear esta relación se tienen que crear ANCLAS dentro de las clases. Las anclas son REFERENCIAS, por lo tanto, si se quiere tener una referencia de libro dentro de precio, se tiene que crear una referencia de la clase Book dentro de Price
         */
		public Book Book { get; set; }
	}
}
