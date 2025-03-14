using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.DataConfig;
using BreweryAPI.Models;
using BreweryAPI.Models.Auth;
using BreweryAPI.Services;
using BreweryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

//Dependency Injection
#region Repositories
builder.Services.AddScoped<IRepository<Brewery>, Repository<Brewery>>();
builder.Services.AddScoped<IRepository<Beer>, Repository<Beer>>();
builder.Services.AddScoped<IRepository<BreweryStock>, Repository<BreweryStock>>();
builder.Services.AddScoped<IRepository<Wholesaler>, Repository<Wholesaler>>();
builder.Services.AddScoped<IRepository<WholesalerStock>, Repository<WholesalerStock>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
#endregion

#region Utilities
builder.Services.AddAutoMapper(typeof(DefaultMappingProfile));
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
            ValidateIssuerSigningKey = true
        };
    });
#endregion

#region Services
builder.Services.AddTransient<BaseCRUDService<Brewery, BreweryDTO>, BaseCRUDService<Brewery, BreweryDTO>>();
builder.Services.AddTransient<BaseCRUDService<Beer, BeerDTO>, BaseCRUDService<Beer, BeerDTO>>();
builder.Services.AddTransient<BaseCRUDService<Wholesaler, WholesalerDTO>, BaseCRUDService<Wholesaler, WholesalerDTO>>();
builder.Services.AddTransient<IBreweryService, BreweryService>();
builder.Services.AddTransient<IAuthService, AuthService>();
#endregion


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
