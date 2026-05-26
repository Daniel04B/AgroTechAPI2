using AgroTechAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. REGISTRO DE SERVICIOS (Dependency Injection)
// ==========================================
builder.Services.AddControllers();

// ¡AQUÍ SE AGREGA SWAGGER!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la conexión a SQL Server
builder.Services.AddDbContext<AgroTechContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AgroTechConnection")));

// 1. CONFIGURACIÓN DE SERVICIOS (Va antes de var app = builder.Build();)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7297", "http://localhost:7297") // Tu puerto real de Blazor
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Cachea la pre-petición para evitar bloqueos
    });
});

var app = builder.Build();

// 2. PIPELINE DE MIDDLEWARES (El orden aquí abajo es CRÍTICO)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// !!! EL CORS DEBE IR AQUÍ: ANTES DE CUALQUIER CONTROLADOR O ENRUTAMIENTO !!!
app.UseCors("AllowBlazor");

app.UseAuthorization();

app.MapControllers();

app.Run();