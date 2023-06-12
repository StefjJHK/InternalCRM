using BIP.InternalCRM.Application;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.Persistence;
using BIP.InternalCRM.WebIdentity;
using BIP.InternalCRM.WebIdentity.Persistence;
using BIP.InternalCRM.WebIdentity.Roles;
using BIP.InternalCRM.WebIdentity.Users;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var config = builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services
    .AddControllers()
    .AddProblemDetailsConventions()
    .AddNewtonsoftJson();

builder.Services
    .AddProblemDetails(opts =>
    {
        opts.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    });

builder.Services
    .AddSwaggerGen()
    .AddSwaggerGenNewtonsoftSupport();

builder.Services
    .RegisterApplicationServices(config)
    .RegisterInfrastructureServices()
    .RegisterPersistenceServices(config) 
    .AddDbContext<AuthDbContext>(opts =>
    {
        opts.UseSqlServer(config.GetConnectionString(DbConstants.DbConnectionString));
    })
    .AddMediatR(opts => opts.RegisterServicesFromAssembly(WebIdentityProject.AssemblyRef))
    .AddTransient<IUserService, UserService>()
    .AddTransient<IRoleService, RoleService>();

builder.Services.AddOptions<JwtTokenOptions>()
    .BindConfiguration(nameof(JwtTokenOptions))
    .ValidateDataAnnotations();

builder.Services
    .AddAutoMapper(WebIdentityProject.AssemblyRef, ApplicationProject.AssemblyRef);

builder.Services
    .AddDefaultCorrelationId()
    .AddCorrelationId(opts =>
    {
        config.GetSection(nameof(CorrelationIdOptions)).Bind(opts);
        opts.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
    });

builder.Services
    .AddIdentity<User, Role>(opts => { config.GetSection(nameof(IdentityOptions)).Bind(opts); })
    .AddUserManager<UserService>()
    .AddRoleManager<RoleService>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAppCors(config);

var app = builder.Build();

app.UseProblemDetails();
app.UseCorrelationId();

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

if (config.GetValue<bool>("CorsOptions:Enabled"))
{
    app.UseCors();
}

app.MapControllers();

await app.RunAsync();