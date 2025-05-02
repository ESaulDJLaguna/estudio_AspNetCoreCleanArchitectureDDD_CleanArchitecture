namespace LeerData.Models
{
	public class Comment
	{
		public int CommentId { get; set; }
		public string Student { get; set; }
		public int Score { get; set; }
		public string TextComment { get; set; }
		public int BookId { get; set; }
		public Book Book { get; set; }
	}
}
