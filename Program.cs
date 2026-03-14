using MongoDB.Driver;
using BOOKINGAPI.Services;

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
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<BilletService>();
builder.Services.AddSingleton<DestinationService>();
builder.Services.AddSingleton<ReservationService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMvc");

app.UseAuthorization();

app.MapGet("/", () => "BOOKINGAPI marche bien");

app.MapControllers();

app.Run();