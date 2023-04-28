using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Commands;
using Infrastructure.Persistance;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(policy =>
{
    policy.AddDefaultPolicy(options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expreso de las diez - Microservicio de Autenticación", Version = "v1" });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
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
builder.Services.AddTransient<ITokenServices, TokenServices>();
builder.Services.AddTransient<IUserApiServices, UserApiServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
