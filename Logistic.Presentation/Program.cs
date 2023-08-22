using Logistic;
using Logistic.Application;
using Logistic.Filters;
using Logistic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
if (connectionString != null)
    builder.Services.AddDbContext(connectionString, "Logistic.Presentation");
else
    throw new Exception("Не указана строка подключения");

builder.Services.AddInfrastructureGeneration();
builder.Services.AddInfrastructureDependencies();

builder.Services.AddBusinessGeneration();
builder.Services.AddBusinessDependencies();


builder.Services.AddControllers(options => options.Filters.Add(typeof(ActionResponseFilter)));
builder.Services.AddControllersGeneration();
builder.Services.AddPresentationDependencies();

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

