-- Relaci�n 1:1
ALTER TABLE Prices
-- CONSTRAINT: es una regla que se le agregar� a una tabla en particular. En este caso agregaremos una regla (CONSTRAINT) que me indique que la clave for�nea de la tabla Prices ser� su columna BookId
ADD CONSTRAINT FK_PRICE_BOOK -- Nombre de la regla
FOREIGN KEY (BookId) -- La "regla" ser� una llave foranea y se le indica qu� columna ser� la llave for�nea
REFERENCES Books(BookId); -- Indicamos a qu� tabla (Books) est� haciendo referencia dicha llave for�nea y cu�l es la primary key de dicha tabla (BookId)



-- Relaci�n 1:m
ALTER TABLE Comments
	ADD CONSTRAINT FK_COMMENT_BOOK
		FOREIGN KEY (BookId) REFERENCES Books(BookId);



-- Relaci�n m:m
-- Tanto BookId como AuthorId son la llave primaria (en conjunto). Ahora se crear�n las relaciones, ya que a pesar de que son llaves primarias, tambi�n deben ser llaves for�neas para que esta tabla se relacione con las tablas Book y Author
ALTER TABLE AuthorsBooks
	ADD CONSTRAINT FK_AUTHOR_BOOK_BOOKID
		FOREIGN KEY(BookId) REFERENCES Books(BookId);
ALTER TABLE AuthorBook
	ADD CONSTRAINT FK_AUTHOR_BOOK_AUTHORID
		FOREIGN KEY(AuthorId) REFERENCES Authors(AuthorId);
