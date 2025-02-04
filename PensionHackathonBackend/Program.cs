using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PensionHackathonBackend.Application.Services;
using PensionHackathonBackend.Core.Abstractions;
using PensionHackathonBackend.DataAccess;
using PensionHackathonBackend.DataAccess.Repositories;
using PensionHackathonBackend.Extensions;
using PensionHackathonBackend.Infrastructure;
using PensionHackathonBackend.Infrastructure.Abstraction;
using PensionHackathonBackend.Logs;

namespace PensionHackathonBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddApiAuthentication(configuration);
        services.AddSwaggerGen();
        
        builder.Services.AddMemoryCache();

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        
        builder.Services.AddDbContext<PensionHackathonDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        services.AddEndpointsApiExplorer();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserService>();

        services.AddScoped<IFileServiceRepository, FileServiceRepository>();
        services.AddScoped<FileService>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddCors(options =>
        {
            options.AddPolicy("AspNetApp", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://192.168.0.122:3333", "http://192.168.10.148:3333", "http://192.168.85.148:3333");
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowCredentials();
            });
        });

        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "PensionFundAntiforgery";
            options.HeaderName = "X-CSRF-TOKEN";
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
               // c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCookiePolicy(new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCors("AspNetApp");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.AddMappedEndpoints();

        app.MapGet("/", () => "Hello ForwardedHeadersOptions!");

        app.Run();
    }
}