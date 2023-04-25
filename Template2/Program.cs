using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Commands;
using Infrastructure.Persistance;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expreso de las diez - Microservicio de autenticación", Version = "v1" });
});


//Custom
var connectionString = "";
var gab = "C:\\Users\\Gabo\\Documents\\Backup\\unaj\\ProyectoDeSoftware_1\\2023-Primer-cuatri\\Grupal\\AppDeCitas\\AuthMicroService\\Template2\\Template2";

if (Directory.GetCurrentDirectory() == gab)
{
    // Agregense si usan MSSQL en docker
    connectionString =
        builder.Configuration["ConnectionString2"];
}
else
{
    // MSSQL running locally
    connectionString = builder.Configuration["ConnectionString"];
}

builder.Services.AddDbContext<ExpresoDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IAuthCommands, AuthCommands>();
builder.Services.AddTransient<IAuthQueries, AuthQueries>();
builder.Services.AddTransient<IEncryptServices, EncryptServices>();
builder.Services.AddTransient<IAuthServices, AuthServices>();
builder.Services.AddTransient<IValidateServices, ValidateServices>();



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
