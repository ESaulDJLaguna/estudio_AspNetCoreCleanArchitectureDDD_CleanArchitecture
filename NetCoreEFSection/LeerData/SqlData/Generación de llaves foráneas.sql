-- Relación 1:1
ALTER TABLE Prices
-- CONSTRAINT: es una regla que se le agregará a una tabla en particular. En este caso agregaremos una regla (CONSTRAINT) que me indique que la clave foránea de la tabla Prices será su columna BookId
ADD CONSTRAINT FK_PRICE_BOOK -- Nombre de la regla
FOREIGN KEY (BookId) -- La "regla" será una llave foranea y se le indica qué columna será la llave foránea
REFERENCES Books(BookId); -- Indicamos a qué tabla (Books) está haciendo referencia dicha llave foránea y cuál es la primary key de dicha tabla (BookId)



-- Relación 1:m
ALTER TABLE Comments
	ADD CONSTRAINT FK_COMMENT_BOOK
		FOREIGN KEY (BookId) REFERENCES Books(BookId);



-- Relación m:m
-- Tanto BookId como AuthorId son la llave primaria (en conjunto). Ahora se crearán las relaciones, ya que a pesar de que son llaves primarias, también deben ser llaves foráneas para que esta tabla se relacione con las tablas Book y Author
ALTER TABLE AuthorsBooks
	ADD CONSTRAINT FK_AUTHOR_BOOK_BOOKID
		FOREIGN KEY(BookId) REFERENCES Books(BookId);
ALTER TABLE AuthorBook
	ADD CONSTRAINT FK_AUTHOR_BOOK_AUTHORID
		FOREIGN KEY(AuthorId) REFERENCES Authors(AuthorId);
