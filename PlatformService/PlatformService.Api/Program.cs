using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using PlatformService.Api.AsyncDataServices.Implementations;
using PlatformService.Api.AsyncDataServices.Interfaces;
using PlatformService.Api.Data;
using PlatformService.Api.Data.Prepare;
using PlatformService.Api.Data.Repository.Implementation;
using PlatformService.Api.Data.Repository.Interface;
using PlatformService.Api.SyncDataServices.Http.Implementation;
using PlatformService.Api.SyncDataServices.Http.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if(builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using Sql Server Db");
    builder.Services.AddDbContext<AppDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConnection"));
    });
}
else
{
    Console.WriteLine("--> Using InMemory Db");
    builder.Services.AddDbContext<AppDbContext>(options => {
        options.UseInMemoryDatabase("InMemory");
    });
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Console.WriteLine($"--> CommandsService Endpoint {builder.Configuration["CommandsService"]}");
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

PrepareDataBase.PreparePopulation(app, app.Environment.IsProduction());

app.Run();
