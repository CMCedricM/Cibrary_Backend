using System.Security.Claims;
using Cibrary_Backend.Contexts;
using Cibrary_Backend.Repository;
using Cibrary_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Auth0Net.DependencyInjection;
using Cibrary_Backend.Models;


var builder = WebApplication.CreateBuilder(args);

// Auth 0 Variables
var AUTH0_DOMAIN = Environment.GetEnvironmentVariable("Auth0_Domain");
var AUTH0_DOMAIN_FULL = $"https://{AUTH0_DOMAIN}/";
var AUTH0_JWT_AUDIENCE = Environment.GetEnvironmentVariable("Auth0_JWT_Audience");
var AUTH0_CLIENT_ID = Environment.GetEnvironmentVariable("Auth0_ClientId");
var AUTH0_CLIENT_SECRET = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
var AUTH0_AUDIENCE_MANAGEMENT = Environment.GetEnvironmentVariable("Auth0_Management_Audience");
// Auth0 JWT Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = AUTH0_DOMAIN_FULL;
    options.Audience = AUTH0_JWT_AUDIENCE;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// Mangement API
// builder.Services.AddAuth0AuthenticationClientCore(AUTH0_DOMAIN);
builder.Services.AddAuth0AuthenticationClient(config =>
{
    config.Domain = AUTH0_DOMAIN;
    config.ClientId = AUTH0_CLIENT_ID;
    config.ClientSecret = AUTH0_CLIENT_SECRET;
});

builder.Services.AddAuth0ManagementClient().AddManagementAccessToken(config =>
{
    config.Audience = AUTH0_AUDIENCE_MANAGEMENT;
});


// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UsersServices>();
builder.Services.AddScoped<UserUpdateAuth0Services>();

builder.Services.AddScoped<BooksRepository>();
builder.Services.AddScoped<BooksServices>();

builder.Services.AddScoped<CirculationRepository>();
builder.Services.AddScoped<CirculationServices>();

builder.Services.AddScoped<BookCopyRepository>();


builder.Services.AddHttpClient();

// Database contexts
var connectString = Environment.GetEnvironmentVariable("DATABASE_URL_DOTNET") ?? throw new InvalidOperationException("Database string missing");

builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseNpgsql(connectString, o => o.MapEnum<UserRole>("user_status")));

builder.Services.AddDbContext<BookDBContext>(options =>
    options.UseNpgsql(connectString));

builder.Services.AddDbContext<CirculationDBContext>(options =>
    options.UseNpgsql(connectString, o => o.MapEnum<BookStatus>("book_status")));

builder.Services.AddDbContext<BookCopyDBContext>(options =>
    options.UseNpgsql(connectString, o => o.MapEnum<BookStatus>("book_status")));


// Cors Rules
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://localhost:7071", "https://cibrary.vercel.app").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }
    );
});



var app = builder.Build();


app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
