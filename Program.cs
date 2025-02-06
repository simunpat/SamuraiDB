using Microsoft.EntityFrameworkCore;
using EntityFrameworkOpgave.DAL.Repositories;
using EntityFrameworkOpgave.DAL.Models;
using EntityFrameworkOpgave.DAL.Data;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

Console.Clear();

Console.WriteLine("Program starting...");
Console.WriteLine("Go to http://localhost:5000/swagger see the API documentation.");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EntityFrameworkOpgave API", Version = "v1" });
});

// Configure DbContext
builder.Services.AddDbContext<SamuraiDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Register repositories
builder.Services.AddScoped<IRepository<Samurai>, SamuraiRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EntityFrameworkOpgave API V1");
});

app.MapControllers();

app.Run();
