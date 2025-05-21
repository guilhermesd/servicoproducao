using Application.UseCases;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.ServicosExternos;
using Infrastructure.Repositories;
using Infrastructure.ServicosExternos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependências para Repositórios
builder.Services.AddScoped<IProducaoContext, ProducaoContext>();
builder.Services.AddScoped<IProducaoRepository, ProducaoRepository>();
builder.Services.AddScoped<IPedidosService, PedidosService>();
builder.Services.AddHttpClient<IPedidosService, PedidosService>();

// Injeção de dependências para Use Cases
builder.Services.AddScoped<IPedidoProducaoUseCase, PedidoProducaoUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
