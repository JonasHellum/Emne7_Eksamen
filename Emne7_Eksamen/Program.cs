using Emne7_Eksamen.Configurations;
using Emne7_Eksamen.Data;
using Emne7_Eksamen.Extensions;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Mappers;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Races;
using Emne7_Eksamen.Features.Races.Interfaces;
using Emne7_Eksamen.Features.Races.Mappers;
using Emne7_Eksamen.Features.Results;
using Emne7_Eksamen.Features.Results.Interfaces;
using Emne7_Eksamen.Health;
using Emne7_Eksamen.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services
    .AddScoped<IMemberService, MemberService>()
    .AddScoped<IMemberRepository, MemberRepository>()
    .AddScoped<IMapper<Member, MemberDTO>, MemberMapper>()
    .AddScoped<IMapper<Member, MemberRegistrationDTO>, MemberRegistrationMapper>();

builder.Services
    .AddScoped<IRaceService, RaceService>()
    .AddScoped<IRaceRepository, RaceRepository>()
    .AddScoped<IMapper<Race, RaceDTO>, RaceMapper>()
    .AddScoped<IMapper<Race, RaceRegistrationDTO>, RaceRegistrationMapper>();

builder.Services
    .AddScoped<IResultService, ResultService>()
    .AddScoped<IResultRepository, ResultRepository>()
    .AddScoped<IMapper<Result, ResultDTO>, ResultMapper>()
    .AddScoped<IMapper<Result, ResultRegistrationDTO>, ResultRegistrationMapper>();







builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");

builder.Services
    .AddScoped<GokstadAthleticsBasicAuthentication>()
    .Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthenticationOptions"));


// Add dbcontext
builder.Services.AddDbContext<GokstadAthleticsDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33))));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddSwaggerBasicAuthentication();

builder.Services.AddSwaggerGen();


builder.Host.UseSerilog((context, configuration) => 
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseMiddleware<GokstadAthleticsBasicAuthentication>()
    .UseAuthorization();

app.MapControllers();

app.Run();