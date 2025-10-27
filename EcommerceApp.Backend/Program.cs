using System.Text;
using EcommerceApp.Backend.Config;
using EcommerceApp.Backend.Data;
using EcommerceApp.Backend.Middleware;
using EcommerceApp.Backend.Services.Interfaces;
using EcommerceApp.Backend.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Database
// -------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// -------------------------------
// JWT Configuration
// -------------------------------
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<JwtSettings>>().Value
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
        };
    });

// -------------------------------
// CORS
// -------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", policy => policy
            .AllowAnyOrigin()   // 👈 permite cualquier dominio
            .AllowAnyHeader()   // 👈 permite cualquier header
            .AllowAnyMethod()   // 👈 permite GET, POST, PUT, DELETE, etc.
    );
});

// -------------------------------
// Dependency Injection (Services)
// -------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// -------------------------------
// Controllers & Middleware
// -------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------------------
// Middleware pipeline
// -------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandling();     // Middleware de errores globales
app.UseHttpsRedirection();
app.UseCors("AllowFlutterApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicialización de datos (Seed)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedData.InitializeAsync(dbContext);
}

app.Run();
