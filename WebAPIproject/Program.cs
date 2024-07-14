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
//using Microsoft.AspNetCore.Localization;
//using Microsoft.EntityFrameworkCore;
//using MODELS;
//using System.Globalization;
//using System.ComponentModel;

//namespace WebAPIproject
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            //var builder = WebApplication.CreateBuilder(args);

//            //// Add MongoDB configuration from appsettings.json
//            //var configuration = builder.Configuration;
//            //var connectionString = configuration.GetConnectionString("MongoDB");
//            //var databaseName = configuration.GetValue<string>("DatabaseName");

//            //// Configure MongoDB client and database
//            //var mongoClient = new MongoClient(connectionString);
//            //var mongoDatabase = mongoClient.GetDatabase(databaseName);

//            //// Add services to the container.
//            //builder.Services.AddSingleton<IMongoClient>(mongoClient);
//            //builder.Services.AddSingleton(mongoDatabase);
//            //builder.Services.AddScoped<IUserInterface, UserData>();

//            //builder.Services.AddControllers();

//            // Add Swagger configuration
//            //builder.Services.AddSwaggerGen(c =>
//            //{
//            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
//            //});

//            //builder.Services.AddAutoMapper(typeof(ProFilesProj).Assembly);

//            //var app = builder.Build();

//            //// Configure the HTTP request pipeline.
//            //if (app.Environment.IsDevelopment())
//            //{
//            //    app.UseSwagger();
//            //    app.UseSwaggerUI(c =>
//            //    {
//            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
//            //    });
//            //}

//            //app.UseHttpsRedirection();

//            //app.UseAuthorization();

//            //app.MapControllers();

//            //app.Run();



//            var builder = WebApplication.CreateBuilder(args);
//            builder.Services.Configure<RequestLocalizationOptions>(options =>
//            {
//                options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
//            });

//            // Add services to the container.
//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            // Register IUser service

//            builder.Services.AddScoped<IUserInterface, UserData>();
//            // Add DbContext
//            builder.Services.AddControllersWithViews();
//            builder.Services.AddDbContext<ProjectContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDataBase")));

//            // Add AutoMapper with the mapping profile
//            builder.Services.AddAutoMapper(typeof(ProFilesProj).Assembly);

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseAuthorization();
//            app.MapControllers();
//            app.Run();
//        }
//    }
//}




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
//using Microsoft.AspNetCore.Localization;
//using Microsoft.EntityFrameworkCore;
//using MODELS;
//using System.Globalization;
//using System.ComponentModel;
//using MongoDB.Driver;
//using Microsoft.OpenApi.Models; // צריך להוסיף את זה עבור Swagger

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

//            builder.Services.AddControllers();

//            // Add Swagger configuration
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
//            });
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
using System.Globalization;

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
            builder.Services.AddSingleton(mongoDatabase);
            builder.Services.AddSingleton(databaseName); // Add this line
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserInterface, UserData>();

            // Add Swagger configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });

            // Configure AutoMapper using explicit namespace
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}