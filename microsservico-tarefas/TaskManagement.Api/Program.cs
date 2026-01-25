using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURAÇÃO DE SERVIÇOS (DEPENDENCY INJECTION) =====

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() {
        Title = "Task Management API",
        Version = "v1",
        Description = "Microsserviço de Gerenciamento de Tarefas - Aplicando todos os conceitos: Clean Code, SOLID, DDD, Unit of Work, Result Pattern"
    });
});

// DbContext - InMemory para demonstração
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseInMemoryDatabase("TaskManagementDb"));

// Dependency Injection (DIP/SOLID)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

// CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

// ===== MIDDLEWARE PIPELINE =====

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Mensagem de início
Console.WriteLine("🚀 Task Management API iniciada!");
Console.WriteLine("📖 Documentação: http://localhost:5000");
Console.WriteLine("🎯 Aplicando: Clean Code, SOLID, DDD, Unit of Work, Result Pattern");

app.Run();

// Necessário para testes de integração
public partial class Program { }
