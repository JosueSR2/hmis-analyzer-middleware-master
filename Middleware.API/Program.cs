using Middleware.Services;
using Middleware.Services.Conections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 HttpClient configurado con base URL de OpenELIS
builder.Services.AddHttpClient<ApiClientService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5284"); // ⚠️ Cambia por la URL real de OpenELIS
});

// 🔹 Servicios internos
builder.Services.AddScoped<AnalyzerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
