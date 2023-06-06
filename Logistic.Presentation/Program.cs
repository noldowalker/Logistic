using Logistic;
using Logistic.Application;
using Logistic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
if (connectionString != null)
    builder.Services.AddDbContext(connectionString, "Logistic.Presentation");
else
    throw new Exception("Не указана строка подключения");

builder.Services.AddInfrastructureDependencies();
builder.Services.AddInfrastructureGeneration();

builder.Services.AddBusinessDependencies();
// builder.Services.AddBusinessGeneration();

builder.Services.AddControllers();
builder.Services.AddControllersGeneration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

