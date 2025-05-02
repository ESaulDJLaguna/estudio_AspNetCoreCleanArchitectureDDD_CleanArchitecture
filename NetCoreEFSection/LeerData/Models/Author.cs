namespace LeerData.Models
{
	public class Author
	{
		public int AuthorId { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Degree { get; set; }
		public byte[]? ProfilePicture { get; set; }
		/*
         * Author tiene que tener un arreglo que represente a la entidad AuthorBook y luego, AuthorBook tiene que tener un ancla que represente a Author
         */
		public HashSet<AuthorBook> BookLink { get; set; }
	}
}
