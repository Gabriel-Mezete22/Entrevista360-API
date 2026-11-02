using System.Text.Json.Serialization;
using Aplicacao;
using Aplicacao.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using Repositorio.Contexto;
using Repositorio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opcoes => opcoes.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EntrevistaDbContext>(opcoes =>
    opcoes.UseSqlServer(builder.Configuration.GetConnectionString("Padrao")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IVagaRepositorio, VagaRepositorio>();
builder.Services.AddScoped<ICriterioRepositorio, CriterioRepositorio>();
builder.Services.AddScoped<ICandidatoRepositorio, CandidatoRepositorio>();
builder.Services.AddScoped<IEntrevistaRepositorio, EntrevistaRepositorio>();
builder.Services.AddScoped<IAvaliacaoRepositorio, AvaliacaoRepositorio>();

builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IVagaAplicacao, VagaAplicacao>();
builder.Services.AddScoped<ICandidatoAplicacao, CandidatoAplicacao>();
builder.Services.AddScoped<IEntrevistaAplicacao, EntrevistaAplicacao>();
builder.Services.AddScoped<IAvaliacaoAplicacao, AvaliacaoAplicacao>();

builder.Services.AddProblemDetails();

var origensPermitidas = builder.Configuration.GetSection("Cors:Origens").Get<string[]>()
                        ?? Array.Empty<string>();

builder.Services.AddCors(opcoes =>
{
    opcoes.AddDefaultPolicy(politica =>
    {
        politica.WithOrigins(origensPermitidas)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();
