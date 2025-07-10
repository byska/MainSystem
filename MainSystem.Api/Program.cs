using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.Services.Strategies;

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
