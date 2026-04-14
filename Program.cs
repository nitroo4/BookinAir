using System.Text;
using MongoDB.Driver;
using BOOKINGAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BOOKINGAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS pour autoriser BOOKINGMVC
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5000",
                "https://localhost:5001",
                "http://localhost:5047",
                "https://localhost:7047"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configuration MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoDB:ConnectionString"];
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDB:DatabaseName"];
    return client.GetDatabase(databaseName);
});

// Enregistrement des services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BilletService>();
builder.Services.AddScoped<DestinationService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<OtpService>();

//GenerateKeyJwt
var key = JwtKeyGenerator.GenerateKey();
Console.WriteLine("JWT KEY:");
Console.WriteLine(key);

// Configuration JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMvc");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "BOOKINGAPI marche bien");

app.MapControllers();

app.Run();