//!=>Referencia [1] [Sección 01, 007 . Inserción de record en EFC]
//!=>Referencia [2] [Sección 01, 010. Métodos de EF]
//!=>Referencia [2] [Sección 01, 012. Tracking y Not Tracking]

using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

//! Instancia del DbContext
StreamerDbContext dbContext = new();
//! Primer ejemplo de objeto a insertar en base de datos
//Streamer streamer = new()
//{
//	Nombre = "Netflix",
//	Url = "https://www.netflix.com"
//};
//Streamer streamer = new()
//{
//	Nombre = "Amazon Prime",
//	Url = "https://www.amazonprime.com"
//};

//! [1] SE COLOCA ! PORQUE INDICAMOS (LE ASEGURAMOS) QUE dbContext ESTÁ INSTANCIADO
//dbContext!.Streamers!.Add(streamer);
//await dbContext.SaveChangesAsync();

//List<Video> movies = new List<Video>
//{
//	new Video
//	{
//		Nombre = "Mad Max",
//		StreamerId = streamer.Id,
//	},
//	new Video
//	{
//		Nombre = "Batman",
//		StreamerId = streamer.Id,
//	},
//	new Video
//	{
//		Nombre = "Crepusculo",
//		StreamerId = streamer.Id,
//	},
//	new Video
//	{
//		Nombre = "Citizen Kane",
//		StreamerId = streamer.Id,
//	}
//};

//await dbContext.AddRangeAsync(movies);
//await dbContext.SaveChangesAsync();
//await AddNewRecords();
//QueryStreaming();
//await QueryFilter();
//await QueryMethods();
//await QueryLinq();
//await TrackingAndNoTracking();
//await AddNewStreamerWithVideo();
//await AddNewStreamerWithVideoId();
//await AddNewActorWithVideo();
//await AddNewDirectorWithVideo();
await MultipleEntitiesQuery();


//! Evitamos que se cierre la consolta una vez termine la ejecución del programa
Console.WriteLine("Presione cualquier tecla para termianr el programa");
Console.ReadKey();


async Task MultipleEntitiesQuery()
{
	//var videoWithActores = await dbContext!.Videos!
	//	.Include(q => q.Actores).FirstOrDefaultAsync(q => q.Id == 1);
	//var actorsNames = await dbContext!.Actores!.Select(q => q.Nombre).ToListAsync();
	var videoWithDirector = await dbContext!.Videos!
		.Where(q => q.Director != null)
		.Include(q => q.Director)
		.Select(q =>
			new
			{
				Director_Nombre_Completo = $"{q.Director.Nombre} {q.Director.Apellido}",
				Movie = q.Nombre
			}
		)
		.ToListAsync();

    foreach (var pelicula in videoWithDirector)
    {
		Console.WriteLine($"{pelicula.Movie} - {pelicula.Director_Nombre_Completo}");
    }
}

async Task AddNewDirectorWithVideo()
{
	var director = new Director
	{
		Nombre = "Lorenzo",
		Apellido = "Basteri",
		VideoId = 1
	};

	await dbContext.AddAsync(director);
	await dbContext.SaveChangesAsync();
}

async Task AddNewActorWithVideo()
{
	var actor = new Actor
	{
		Nombre = "Brand",
		Apellido = "Pitt"
	};

	await dbContext.AddAsync(actor);
	await dbContext.SaveChangesAsync();

	var videoActor = new VideoActor
	{
		ActorId = actor.Id,
		VideoId = 1
	};

	await dbContext.AddAsync(videoActor);
	await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithVideoId()
{
	var batmanForever = new Video
	{
		Nombre = "Batman Forever",
		StreamerId = 4
	};

	await dbContext.AddAsync(batmanForever);
	await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithVideo()
{
	var pantaya = new Streamer
	{
		Nombre = "Pantaya"
	};

	var hungerGames = new Video
	{
		Nombre = "Hunger Games",
		Streamer = pantaya,
	};

	await dbContext.AddAsync(hungerGames);
	await dbContext.SaveChangesAsync();
}

async Task TrackingAndNoTracking()
{
	var streamerWithTracking = await dbContext!.Streamers!
		.FirstOrDefaultAsync(x => x.Id == 1);

	var streamerWithNoTracking = await dbContext!.Streamers!
		.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);

	//! Se actualiza el nombre de ambos registros
	streamerWithTracking.Nombre = "Netflix Super";
	//![3] AL UTILIZAR AsNoTracking() EL OBJETO SE LIBERA DE LA MEMORIA TEMPORAL, POR LO QUE NO ES POSIBLE ACTUALIZARLO
	streamerWithNoTracking.Nombre = "Amazon Prime Videos";

	await dbContext!.SaveChangesAsync();
}

async Task QueryLinq()
{
	Console.WriteLine($"Ingrese el servicio de streaming: ");
	var streamingNombre = Console.ReadLine();
	//! i representa los datos de los campos (las columnas de la entidad). select i, indica que me devuelva todos los campos
	var streamers = await (from i in dbContext.Streamers
						   where EF.Functions.Like(i.Nombre, $"%{streamingNombre}%")
						   select i).ToListAsync();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
	}
}

async Task QueryMethods()
{
	//! Se refactoriza el dbContext común
	var streamer = dbContext!.Streamers!;

	Console.WriteLine("firstAsync");
	//![2] ASUME DE QUE EXISTE LA INFORMACIÓN BUSCADA Y DEVUELVE EL PRIMER REGISTRO, EN CASO DE QUE NO SEA ASÍ, DEVUELVE UNA EXCEPCIÓN
	var firstAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstAsync();
	Console.WriteLine(JsonSerializer.Serialize(firstAsync) + "\n");


	Console.WriteLine("firstOrDefaultAsync");
	//![2] EN CASO DE QUE NO EXISTA LA INFORMACIÓN BUSCADA, DEVUELVE null
	var firstOrDefaultAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstOrDefaultAsync();
	Console.WriteLine(JsonSerializer.Serialize(firstOrDefaultAsync) + "\n");


	Console.WriteLine("firstOrDefaultAsync_v2");
	//![2] OTRA FORMA DE UTILIZAR FirstOrDefaultAsync: DIRECTAMENTE BUSCA EL PRIMER REGISTRO QUE CUMPLA CON LA CONDICIÓN, DE NO ENCONTRARSE DEVUELVE null
	var firstOrDefaultAsync_v2 = await streamer.FirstOrDefaultAsync(y => y.Nombre.Contains("a"));
	Console.WriteLine(JsonSerializer.Serialize(firstOrDefaultAsync_v2) + "\n");


	Console.WriteLine("singleAsync");
	//![2] DEVUELVE UN SOLO REGISTRO, LA DIFERENCIA CON First ES QUE SI LA CONSULTA DEL Where ENCUENTRA VARIOS REGISTROS O NO DEVUELVE NINGUNO, SingleAsync DEVOLVERÁ UNA EXCEPCIÓN
	var singleAsync = await streamer.Where(x => x.Id == 1).SingleAsync();
	Console.WriteLine(JsonSerializer.Serialize(singleAsync) + "\n");


	Console.WriteLine("singleOrDefaultAsync");
	//![2] IGUAL QUE SingleAsync, PERO SI NO ENCUENTRA NINGÚN RESULTADO DEVUELVE null, NO UNA EXCEPCIÓN
	var singleOrDefaultAsync = await streamer.Where(x => x.Id == 1).SingleOrDefaultAsync();
	Console.WriteLine(JsonSerializer.Serialize(singleOrDefaultAsync) + "\n");


	Console.WriteLine("findAsync");
	//![2] BUSCA EN LA TABLA EL REGISTRO CON EL PrimaryKey QUE COINCIDA CON EL PARÁMETRO Y SI NO EXISTE, DEVULVE NULL
	var findAsync = await streamer.FindAsync(3);
	Console.WriteLine(JsonSerializer.Serialize(findAsync) + "\n");


	Console.WriteLine("countAsync");
	//! Recupera el número de elementos en una secuencia
	var countAsync = await streamer.CountAsync();
	Console.WriteLine(JsonSerializer.Serialize(countAsync) + "\n");


	Console.WriteLine("longCountAsync");
	//! Regresa un long que representa el número total de elementos de una secuencia 
	var longCountAsync = await streamer.LongCountAsync();
	Console.WriteLine(JsonSerializer.Serialize(longCountAsync) + "\n");


	Console.WriteLine("minAsync");
	//! Devuelve el valor mínimo del campo seleccionado de la tabla
	var minAsync = await streamer.MinAsync(x => x.Id);
	Console.WriteLine(JsonSerializer.Serialize(minAsync) + "\n");


	Console.WriteLine("maxAsync");
	//! Devuelve el valor máximo del campo seleccionado de la tabla
	var maxAsync = await streamer.MaxAsync(x => x.Id);
	Console.WriteLine(JsonSerializer.Serialize(maxAsync) + "\n");


	Console.WriteLine("Where().Select().FirstOrDefaultAsync()");
	//! Regresa el valor máximo de una secuencia
	var selectValue = await streamer
		.Where(x => x.Nombre.Contains("net")).Select(x => x.Nombre).FirstOrDefaultAsync();
	Console.WriteLine(JsonSerializer.Serialize(selectValue) + "\n");
}

async Task QueryFilter()
{
	Console.WriteLine($"Ingrese una compañía de streaming: ");
	var streamingNombre = Console.ReadLine();
	//! Recordemos que con ! estamos confirmando que dbContext y Streamers no son nulos
	var streamers = await dbContext!.Streamers!
		.Where(x =>
			// x.Nombre == "Netflix"
			// x.Nombre ==streamingNombre
			x.Nombre.Equals(streamingNombre)
		).ToListAsync();

    foreach (var streamer in streamers)
    {
		Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
	}

	//var streamerPartialResults = await dbContext!.Streamers!
	//	.Where(x => x.Nombre.Contains(streamingNombre)).ToListAsync();
	var streamerPartialResults = await dbContext!.Streamers!
		.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingNombre}%")).ToListAsync();

	foreach (var streamer in streamerPartialResults)
	{
		Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
	}
}

void QueryStreaming()
{
	var streamers = dbContext.Streamers!.ToList();
	foreach (var streamer in streamers)
	{
		Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
	}
}

async Task AddNewRecords()
{
	Streamer streamer = new()
	{
		Nombre = "Disney+",
		Url = "https://www.disneyplus.com"
	};

	dbContext!.Streamers!.Add(streamer);
	await dbContext.SaveChangesAsync();

	List<Video> movies = new List<Video>
	{
		new Video
		{
			Nombre = "La Cenicienta",
			StreamerId = streamer.Id,
		},
		new Video
		{
			Nombre = "1001 Dalmatas",
			StreamerId = streamer.Id,
		},
		new Video
		{
			Nombre = "El Jorobado de Notredame",
			StreamerId = streamer.Id,
		},
		new Video
		{
			Nombre = "Star Wars",
			StreamerId = streamer.Id,
		}
	};

	await dbContext.AddRangeAsync(movies);
	await dbContext.SaveChangesAsync();
}