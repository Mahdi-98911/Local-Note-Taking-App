using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoteApp.Core.Data;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

// Set database path explicitly
var dbPath = Path.Combine(AppContext.BaseDirectory, "notes.db");
builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var host = builder.Build();

using(var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
    await db.Database.MigrateAsync();
}

await host.RunAsync();
