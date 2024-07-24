//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using MongoDB.Driver;
//using Microsoft.OpenApi.Models;
//using DAL.Interfaces;
//using DAL.Data;
//using DAL.PROFILES;
//using AutoMapper;
//using MODELS;
//using System.Globalization;

//namespace WebAPIproject
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add MongoDB configuration from appsettings.json
//            var configuration = builder.Configuration;
//            var connectionString = configuration.GetConnectionString("MongoDB");
//            var databaseName = configuration.GetValue<string>("DatabaseName");

//            // Configure MongoDB client and database
//            var mongoClient = new MongoClient(connectionString);
//            var mongoDatabase = mongoClient.GetDatabase(databaseName);

//            // Add services to the container.
//            builder.Services.AddSingleton<IMongoClient>(mongoClient);
//            builder.Services.AddSingleton(mongoDatabase);
//            builder.Services.AddSingleton(databaseName); // Add this line
//            builder.Services.AddControllers();
//            builder.Services.AddScoped<IUserInterface, UserData>();
//            builder.Services.AddScoped<IReactionInterface, ReactionData>();
//            builder.Services.AddScoped<IResortInterface, ResortData>();
//            // Add CORS service
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAll", builder =>
//                {
//                    builder.AllowAnyOrigin()
//                           .AllowAnyMethod()
//                           .AllowAnyHeader();
//                });
//            });

//            // Add Swagger configuration
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
//            });

//            // Configure AutoMapper using explicit namespace
//            builder.Services.AddAutoMapper(typeof(ProFilesProj).Assembly);
//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI(c =>
//                {
//                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
//                });
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using DAL.Interfaces;
using DAL.Data;
using DAL.PROFILES;
using AutoMapper;
using MODELS;

namespace WebAPIproject
{
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
            builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase); // Specify IMongoDatabase instead of string
            builder.Services.AddSingleton(databaseName); // This might not be necessary unless used elsewhere
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserInterface, UserData>();
            builder.Services.AddScoped<IReactionInterface, ReactionData>();
            builder.Services.AddScoped<IResortInterface, ResortData>();

            // Add CORS service
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add Swagger configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });

            // Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(ProFilesProj));

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
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
