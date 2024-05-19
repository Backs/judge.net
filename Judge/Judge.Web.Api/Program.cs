using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Judge.Web.Api;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Judge.Web.Api.xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Judge.Web.Client.xml"));

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Judge.NET", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        builder.Services.AddServices(builder.Configuration["AppSettings:DatabaseConnectionString"]);
        builder.Services.AddTransient<ErrorHandlerMiddleware>();

        var key = builder.Configuration["AppSettings:SecurityKey"];
        var issuer = builder.Configuration["AppSettings:Issuer"];
        var audience = builder.Configuration["AppSettings:Audience"];

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.AdminPolicy,
                policy =>
                {
                    policy.Requirements.Add(new ClaimsAuthorizationRequirement(ClaimTypes.Role, new[] { "admin" }));
                });
        });

        builder.Services.AddCors();
        
        var app = builder.Build();

        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCors(policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());


        app.MapFallbackToFile("index.html");

        app.Run();
    }
}