using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using TaskGarden.Api;
using TaskGarden.Api.Application.Features.Auth;
using TaskGarden.Api.Application.Features.Categories;
using TaskGarden.Api.Application.Features.Invitation;
using TaskGarden.Api.Application.Features.TaskList;
using TaskGarden.Api.Application.Features.TaskListItem;
using TaskGarden.Api.Application.Shared.Extensions;
using TaskGarden.Api.Domain.Entities;
using TaskGarden.Api.Infrastructure.DependencyInjection;
using TaskGarden.Api.Infrastructure.Middlewares;
using TaskGarden.Api.Infrastructure.Persistence;
using TaskGarden.Application.Configurations;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var certFilePath = Environment.GetEnvironmentVariable("CERT_FILE_PATH");  
    var certPassword = Environment.GetEnvironmentVariable("CERT_PASSWORD");    

    Console.WriteLine($"Certificate File Path: {certFilePath}");
    Console.WriteLine($"Certificate Password: {certPassword}");
    
    if (string.IsNullOrEmpty(certFilePath) || string.IsNullOrEmpty(certPassword))
    {
        throw new InvalidOperationException("SSL certificate path or password is not set.");
    }

    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(certFilePath, certPassword);
    });
});



// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

builder.Services.AddCorsService();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddIdentityService();
builder.Services.AddApplicationServices();
builder.Services.AddEmailService();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add ProblemDetails services before any middleware is called
builder.Services.AddProblemDetails(ExceptionExtensions.ConfigureProblemDetails);

var app = builder.Build();

app.UseMiddleware<JwtTokenMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddSecurityHeaders();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// 🔒 Redirect HTTP to HTTPS (safe to keep, will be ignored if no HTTP port)
app.UseHttpsRedirection();

app.MapAuthEndpoints();
app.MapCategoryEndpoints();
app.MapTaskListEndpoints();
app.MapTaskListItemEndpoints();
app.MapInvitationEndpoints();
app.MapTasklistMemberEndpoints();

app.Run();
