using AdventureWorksClassLib.Services;
using AdventureWorksClassLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using AdventureWorksAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DB Context
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(
       builder.Configuration.GetConnectionString("AdventureWorks")));
//Class library
builder.Services.AddScoped<AuthClass>();
builder.Services.AddScoped<ProductClass>();
//Auth 
builder.Services.AddAuthentication("Auth")
                .AddScheme<AuthenticationSchemeOptions, 
                            AuthHandler>("Auth", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
