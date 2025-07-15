using MainSystem.Application.Abstraction;
using MainSystem.Application.UseCases.FlightRosterUseCases.Queries;
using MainSystem.Domain.Services.Builders;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.Services.Strategies;
using MainSystem.Infrastructure.External.Adapters;
using MainSystem.Infrastructure.Identity;
using MainSystem.Infrastructure.Persistence;
using MainSystem.Infrastructure.Repositories;
using MainSystem.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetRosterByIdQueryHandler>());

builder.Services.AddDbContext<MainSystemDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddScoped<IFlightRosterRepository, FlightRosterRepository>();
builder.Services.AddTransient<FlightRosterBuilder>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services
    .AddIdentityCore<ApplicationUser>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MainSystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var cfg = builder.Configuration;
        opt.TokenValidationParameters = new()
        {
            ValidIssuer = cfg["Jwt:Issuer"],
            ValidAudience = cfg["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                   Encoding.UTF8.GetBytes(cfg["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanCreateRoster", p => p.RequireRole( "Admin"));
    options.AddPolicy("CanViewRoster", p => p.RequireRole("Viewer", "Admin"));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await SeedRolesAndUsersAsync.SeedAsync(scope.ServiceProvider);
// Configure the HTTP request pipeline.

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
