using Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddUseCases()
    .AddAppConnections()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Classe parcial adicionada para injetar no teste integrado
public partial class Program { };