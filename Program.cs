﻿using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;




var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensión personalizada
builder.Services.AddSwaggerGen();

// 🔒 Cadena de conexión directamente en el código
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos", policy =>
    {
        policy.AllowAnyOrigin()   // Permitir cualquier origen
              .AllowAnyHeader()   // Permitir cualquier encabezado
              .AllowAnyMethod();  // Permitir cualquier método (GET, POST, etc.)
    });
});


// Servicios personalizados (como validadores, etc.)
builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi(); // OpenAPI personalizado
}

// Habilitar CORS
app.UseCors("PermitirTodos");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
