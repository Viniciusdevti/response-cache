using ResponseCache.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<INumberService, NumberService>();
builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services.AddResponseCaching();

var app = builder.Build();
app.MapControllers();
app.UseResponseCaching();
app.Run();
