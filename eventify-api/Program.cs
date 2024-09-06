using eventify_api.Interfaces;
using eventify_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add configuration settings from user secrets
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>();

// Retrieve frontend origin from user secrets or appsettings
var frontendOrigin = builder.Configuration["FrontendOrigin"];

// Custom services
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();
builder.Services.AddSingleton<IEventService, EventService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    // Development CORS policy (allow any origin)
    options.AddPolicy("DevelopmentCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    // Production CORS policy (allow specific origin)
    options.AddPolicy("ProductionCorsPolicy", builder =>
    {
        builder.WithOrigins(frontendOrigin) // Allow only your React frontend
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentCorsPolicy"); // Use the development CORS policy
}
else
{
    app.UseCors("ProductionCorsPolicy"); // Use the production CORS policy
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Bind to the port provided by Heroku or default to 5000 if not specified
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
