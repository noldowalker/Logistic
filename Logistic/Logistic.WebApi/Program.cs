using System.Reflection;
using Logistic;
using Logistic.Filters;
using Logistic.Application;
using Logistic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");
if (connectionString != null)
    builder.Services.AddInfrastructure(connectionString);
else
    throw new Exception("Не указана строка подключения");

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddPresentation();
builder.Services.AddControllers(options => options.Filters.Add(typeof(ActionResponseFilter)));
builder.Services.AddControllersGeneration();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplication();


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
