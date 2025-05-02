using LeerData;
using LeerData.Models;
using Microsoft.EntityFrameworkCore;

AppBookSaleContext db = new();

// Recuperamos la información dentro de Books
var books = db.Books.AsNoTracking(); // Con AsNoTracking le indicamos que NO guarde la información en caché

foreach (var book in books)
{
	Console.WriteLine($"{book.Title} --- {book.Description}");
}
Console.WriteLine();



// Recuperamos la información dentro de Books así como su información relacionada
var booksWithPrice = db.Books.Include(x => x.PromotionPrice).AsNoTracking();

foreach (var book in booksWithPrice)
{
	Console.WriteLine($"{book.Title}: ${book.PromotionPrice.CurrentPrice}");
}
Console.WriteLine();



var booksWithComments = db.Books.Include(x => x.ListComment).AsNoTracking();

foreach (var book in booksWithComments)
{
	Console.WriteLine($"{book.Title}:");

	foreach (var comment in book.ListComment)
	{
		Console.WriteLine($"----- {comment.Student}: ({comment.Score}) {comment.TextComment}");
	}
	Console.WriteLine();
}



/*
 * Se quiere crear una consulta que retorne la relación de los libros junto con el nombre del autor.
 * 
 * Al hacer una consulta directa a Books, lo que se tiene que hacer, es incluir a la data que está en AuthorsBooks, pero esta tabla NO tiene el nombre del autor, solo tiene el Id del autor, por lo tanto, se si quisiera incluir el nombre, se tendría que hacer otra inclusión extra, incluyendo a la entidad Author y de esa manera traer la data del autor.
 * 
 * Por lo tanto se comenzaría desde Book, luego se incluye a AuthorBook y finalmente incluimos a Author
 */
var booksWithAuthors = db.Books.Include(x => x.AuthorLink).ThenInclude(xi => xi.Author).AsNoTracking();

foreach (var book in booksWithAuthors)
{
	Console.WriteLine($"{book.Title}:");

	foreach (var authLink in book.AuthorLink)
	{
		Console.WriteLine($"---- {authLink.Author.Name} {authLink.Author.LastName}");
	}
}
Console.WriteLine();



// AGREGAR NUEVA INFORMACIÓN CON ENTITYFRAMEWORK CORE
Author newAuthor = new();
Console.Write("Ingresa el nombre del autor (skip): ");
newAuthor.Name = Console.ReadLine();

if (newAuthor.Name != "skip")
{
	Console.Write("Ingresa el apellido: ");
	newAuthor.LastName = Console.ReadLine();
	Console.Write("Ingresa el grado de estudio: ");
	newAuthor.Degree = Console.ReadLine();

	db.Add(newAuthor);
	/*
	 * En este caso el valor devuelto es 1. Este 1 indica que solo hubo una sola transacción ya que solo se agregó un autor. En caso de agregar dos autores, devolvería 2, por lo tanto, el valor devuelto, depende de la cantidad de transacciones que se hace en la base de datos. Esto también pasaría para cualquier tipo de operación que involucre, actualizar, eliminar o como en nuestro caso: agregar nueva información.
	 * 
	 * Para demostrar esto, se hará lo siguiente:
	 */
	//Author newAuthor = new Author
	//{
	//    Name = "Pedro",
	//    LastName = "Paredes",
	//    Degree = "Master"
	//};
	//db.Add(newAuthor);

	//Author newAuthor2 = new Author
	//{
	//	Name = "Paola",
	//	LastName = "Martinez",
	//	Degree = "Master"
	//};
	//db.Add(newAuthor2);

	int addTransactionState = db.SaveChanges();

	Console.WriteLine($"Estado de transacción al agregar ===> {addTransactionState}");
}



// ACTUALIZAR INFORMACIÓN CON ENTITYFRAMEWORK CORE
Console.Write("Id del autor a modificar: ");
int authorIdToEdit = int.Parse(Console.ReadLine());

var authorToUpdate = db.Authors.Single(x => x.AuthorId == authorIdToEdit);

if(authorToUpdate is not null)
{
    Console.Write("Ingresa el nuevo grado de estudio: ");
	authorToUpdate.Degree = Console.ReadLine();

	int updateTransactionState = db.SaveChanges();
	Console.WriteLine($"Estado de transacción al editar ===> {updateTransactionState}");
}



// ELIMINAR INFORMACIÓN CON ENTITYFRAMEWORK CORE
Console.Write("Id del autor a eliminar: ");
int authorIdToDelete = int.Parse(Console.ReadLine());

var authorToDelete = db.Authors.Single(x => x.AuthorId == authorIdToDelete);

if (authorToDelete is not null)
{
	db.Remove(authorToDelete);
	int deleteTransactionState = db.SaveChanges();
	Console.WriteLine($"Estado de transacción al eliminar ===> {deleteTransactionState}");
}