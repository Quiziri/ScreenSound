using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(config =>
{
    var settings = config.Build();
    config.AddAzureAppConfiguration("Endpoint=https://screensound-configurationx.azconfig.io;Id=MECn;Secret=GUpulwjQDAoNt54tlZisiVZ9K52VVrEX6Hwwal8NTmc=");
});

builder.Services.AddDbContext<ScreenSoundContext>((options) =>
{
    options
        .UseSqlServer(builder.Configuration
          ["ConnectionStrings:ScreenSoundDB"])
        .UseLazyLoadingProxies();
});

builder.Services.AddIdentityApiEndpoints<PessoaComAcesso>().AddEntityFrameworkStores<ScreenSoundContext>();

builder.Services.AddAuthorization();

builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7204",
        builder.Configuration["FrontendUrl"] ?? "https://localhost:7015"])
        .AllowAnyMethod()
        .SetIsOriginAllowed(pol => true)
        .AllowAnyHeader()
        .AllowCredentials()));

var app = builder.Build();

app.UseCors("wasm");

app.UseStaticFiles();
app.UseAuthorization();

app.AddEndPointsArtistas();
app.AddEndPointsMusicas();
app.AddEndPointsGeneros();

app.MapGroup("auth").MapIdentityApi<PessoaComAcesso>().WithTags("Autorização");

app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization().WithTags("Autorização");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();