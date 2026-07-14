using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoteApp.Core.Data;
using NoteApp.Core.Repositories;
using NoteApp.Core.Services;
using NoteApp.Core.UI;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

// Set database path explicitly
var dbPath = Path.Combine(AppContext.BaseDirectory, "notes.db");
builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ConsoleMenu>();
var host = builder.Build();

using(var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
    await db.Database.MigrateAsync();
    var menu = scope.ServiceProvider.GetRequiredService<ConsoleMenu>();
    await menu.Run();
}


