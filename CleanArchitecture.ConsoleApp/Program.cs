using CleanArchitecture.Data;
using CleanArchitecture.Domain;

StreamerDbContext dbContext = new();
Streamer streamer = new()
{
	Nombre = "Netflix",
	Url = "https://www.netflix.com"
};
dbContext!.Streamers!.Add(streamer);
await dbContext.SaveChangesAsync();
