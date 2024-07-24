using AutoMapper;
using BL.Middlewares;
using DAL.Data;
using DAL.Interfaces;
using DAL.PROFILES;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Text;
using WebAPIproject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add MongoDB configuration from appsettings.json
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = configuration.GetValue<string>("DatabaseName");

        // Configure MongoDB client and database
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);

        // Add services to the container.
        builder.Services.AddSingleton<IMongoClient>(mongoClient);
        builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
        builder.Services.AddControllers();

        // Configure AutoMapper
        builder.Services.AddAutoMapper(typeof(ProFilesProj));

        // Register services with dependency injection
        builder.Services.AddScoped<IUserInterface>(sp => new UserData(
            sp.GetRequiredService<IMongoClient>(),
            databaseName,
            sp.GetRequiredService<IMapper>()
        ));
        builder.Services.AddScoped<IReactionInterface>(sp => new ReactionData(
            sp.GetRequiredService<IMongoClient>(),
            databaseName,
            sp.GetRequiredService<IMapper>()
        ));
        builder.Services.AddScoped<IResortInterface>(sp => new ResortData(
            sp.GetRequiredService<IMongoClient>(),
            databaseName,
            sp.GetRequiredService<IMapper>()
        ));
        builder.Services.AddScoped<IAuthInterface, RestfulJWTL>();

        // Add CORS service
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Add JWT Authentication
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            };
        });

        // Add Swagger configuration
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(@"C:\Users\שושנה\Documents\לימודים\web API\WebAPIproject\WebAPIprojectLog.txt",
            rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll"); // Apply CORS policy
        app.UseAuthentication(); // Add authentication
        app.UseAuthorization(); // Add authorization
        app.UseMiddleware<JwtMiddleware>(); // Use the JWT Middleware
        app.UseMiddleware<LogMiddleware>(); // Use the Log Middleware
        app.MapControllers();
        app.Run();
    }
}



