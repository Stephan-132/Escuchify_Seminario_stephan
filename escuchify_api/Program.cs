using escuchify_api.Data;
using escuchify_api.Core.Entities;
using escuchify_api.Services; // Para ArtistasService
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- ZONA DE SERVICIOS (Inyección de Dependencias) ---

// 1. Base de Datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=escuchify.db")
);

// 2. Controladores (Esto activa los archivos que acabas de crear)
builder.Services.AddControllers();

// 3. Tus Servicios Propios
builder.Services.AddScoped<ArtistasService>();

// 4. Configuración Extra (Swagger, CORS)
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(option => {
    option.AddPolicy("AllowAll", builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// --- ZONA DE MIDDLEWARE (Configuración de la App) ---

// Crear la BD si no existe (Útil para desarrollo rápido)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // OJO: Si usas migraciones, es mejor usar dbContext.Database.Migrate();
    dbContext.Database.Migrate(); 
}

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ESTA LÍNEA ES MAGIA: Busca todos los Controllers y crea las rutas automáticamente
app.MapControllers(); 

app.Run();