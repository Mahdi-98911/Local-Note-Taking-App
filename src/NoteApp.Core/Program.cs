using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoteApp.Core.Data;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

// Set database path explicitly
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "notes.db");

builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var host = builder.Build();

using(var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
    await db.Database.MigrateAsync();
}

await host.RunAsync();
