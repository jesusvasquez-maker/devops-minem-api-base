using DevOpsMinem.Application.Services;
using DevOpsMinem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DevOpsMinem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Servicios de infraestructura (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=devopsminem.db";

builder.Services.AddInfrastructure(connectionString);

// Servicios de aplicación
builder.Services.AddScoped<UserService>();

// API y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "DevOpsMinem API",
        Version = "v1",
        Description = "API de gestión de usuarios · Curso DevOps Fundamentals · MINEM"
    });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Aplicar migraciones automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger siempre visible (incluso en producción para el curso)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevOpsMinem API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

app.Run();

// Necesario para que xUnit pueda instanciar el Program en tests de integración
public partial class Program { }
