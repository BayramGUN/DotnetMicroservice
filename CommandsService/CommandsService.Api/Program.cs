using CommandsService.Api.AsyncDataServices;
using CommandsService.Api.Data;
using CommandsService.Api.Data.Repositories.Implementations;
using CommandsService.Api.Data.Repositories.Interfaces;
using CommandsService.Api.EvenProcessing.Implementations;
using CommandsService.Api.EvenProcessing.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine("--> Using InMemory Db");
    builder.Services.AddDbContext<AppDbContext>(options => {
        options.UseInMemoryDatabase("InMemory");
    });
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddControllers();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EvenProcessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
