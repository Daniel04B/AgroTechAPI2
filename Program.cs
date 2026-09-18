using AgroTechAPI.Data;
using AgroTechAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------
// Servicios
//--------------------------------------

builder.Services.AddControllers();

builder.Services.AddScoped<JwtService>();

//--------------------------------------
// SQL SERVER
//--------------------------------------

builder.Services.AddDbContext<AgroTechContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AgroTechConnection")));

//--------------------------------------
// JWT
//--------------------------------------

var jwtSettings =
    builder.Configuration.GetSection("Jwt");

builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                jwtSettings["Issuer"],

            ValidAudience =
                jwtSettings["Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSettings["Key"]!))
        };
});

builder.Services.AddAuthorization();

//--------------------------------------
// CORS
//--------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy.WithOrigins(
                "https://localhost:7297",
                "http://localhost:7297")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

//--------------------------------------
// Swagger
//--------------------------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

//--------------------------------------
// Pipeline
//--------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();