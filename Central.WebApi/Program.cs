using Central.Persistence.Extensions;
using Central.Persistence.Seeders;
using Central.Application.Extensions;
using Central.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApiServices();
// builder.Services.AddApiCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAllAsync();
    await seeder.RunFakersAsync();
}

app.UseApiMiddleware();
// app.UseApiCors();

app.MapApiEndpoints();

app.Run();