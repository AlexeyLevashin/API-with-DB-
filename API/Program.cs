using API.Extensions;
using Application.Extensions;
using FluentMigrator.Runner;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load("../.env");
}
var connectionString = builder.Configuration["ConnectionStrings:Database"];

builder.Services.AddJwtTokenBearer();
builder.Services.AddSwaggerWithAuth();
builder.Services.AddSwaggerGen();
builder.Services.AddMigrations(connectionString);
builder.Services.AddDapper();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();

var app = builder.Build();
var serviceProvider = app.Services.CreateScope().ServiceProvider;
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.MapControllers();
app.MapSwagger();
app.UseSwaggerUI();

app.Run();