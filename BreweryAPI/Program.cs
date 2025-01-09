using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.DataConfig;
using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

//Dependency Injection
#region Repositories
builder.Services.AddScoped<IRepository<Brewery>, Repository<Brewery>>();
builder.Services.AddScoped<IRepository<Beer>, Repository<Beer>>();
builder.Services.AddScoped<IRepository<BreweryStock>, Repository<BreweryStock>>();
builder.Services.AddScoped<IRepository<Wholesaler>, Repository<Wholesaler>>();
builder.Services.AddScoped<IRepository<WholesalerStock>, Repository<WholesalerStock>>();
#endregion

builder.Services.AddAutoMapper(typeof(DefaultMappingProfile));

#region Services
builder.Services.AddTransient<BaseCRUDService<Brewery, BreweryDTO>, BreweryService>();
builder.Services.AddTransient<BaseCRUDService<Beer, BeerDTO>, BeerService>();
#endregion

builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


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
