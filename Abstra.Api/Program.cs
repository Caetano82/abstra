using System.Text;
using Abstra.Repository.Data;
using Abstra.Repository.Repositories;
using Abstra.Api.Middleware;
using Abstra.Domain.Contracts;
using Abstra.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Abstra API",
        Version = "v1",
        Description = "REST API for managing Countries and Cities"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=AbstraDb;Trusted_Connection=true;TrustServerCertificate=true";

builder.Services.AddDbContext<AbstraDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository Registration
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

// Service Registration
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// MediatR Configuration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Abstra.Application.Services.CountryService).Assembly));

// JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
var issuer = jwtSettings["Issuer"] ?? "AbstraApi";
var audience = jwtSettings["Audience"] ?? "AbstraApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5174")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply database migrations (skip for in-memory databases used in tests)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AbstraDbContext>();
    if (dbContext.Database.IsRelational() && dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
    {
        dbContext.Database.Migrate();
    }
}

app.Run();
