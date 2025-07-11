using MainSystem.Application.Abstraction;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.Services.Strategies;
using MainSystem.Infrastructure.External.Adapters;
using MainSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ISeatAssignmentStrategy, GreedySeatAssignmentStrategy>();
builder.Services.AddSingleton<ISeatAssignmentStrategy, GroupAwareSeatAssignmentStrategy>();
builder.Services.AddSingleton<ISeatAssignmentStrategy, RandomSeatAssignmentStrategy>();
builder.Services.AddSingleton<ISeatAssignmentStrategyFactory, SeatAssignmentStrategyFactory>();

builder.Services.AddHttpClient<IPassengerAdapter, PassengerHttpAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Endpoints:PassengerApi"]));

builder.Services.AddHttpClient<ICabinCrewAdapter, CabinCrewHttpAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Endpoints:CabinCrewApi"]));

builder.Services.AddHttpClient<IPilotPoolAdapter, PilotPoolAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Endpoints:PiloApi"]));
builder.Services.AddHttpClient<IFlightInfoAdapter, FlightInfoHttpAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Endpoints:FlightInfoApi"]));

builder.Services.AddScoped<ICabinCrewPoolRepository, CabinCrewRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IPilotPoolRepository, PilotPoolRepository>();
builder.Services.AddScoped<IFlightInfoRepository, FlightInfoRepository>();

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
