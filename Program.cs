using System.Security.Claims;
using Cibrary_Backend.Contexts;
using Cibrary_Backend.Repository;
using Cibrary_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<UsersServices>();

builder.Services.AddScoped<BooksRepository>();
builder.Services.AddScoped<BooksServices>();

var connectString = Environment.GetEnvironmentVariable("DATABASE_URL_DOTNET");

builder.Services.AddDbContext<UsersDBContext>(options =>
    options.UseNpgsql(connectString ?? throw new InvalidOperationException("Database string missing")));

builder.Services.AddDbContext<BooksDBContext>(options =>
    options.UseNpgsql(connectString ?? throw new InvalidOperationException("Database string missing")));

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
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
