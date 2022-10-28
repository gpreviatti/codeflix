using Api.Configurations;
using Api.Filters;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (connectionString is null)
    connectionString = "Server=localhost;Port=3306;Uid=root;Pwd=codeflix;Database=catalog_dev;";

builder.Services
    .AddUseCases()
    .AddAppConnections(connectionString)
    .AddControllers(options => options.Filters.Add(typeof(ApiGlobalExceptionFilter)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
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