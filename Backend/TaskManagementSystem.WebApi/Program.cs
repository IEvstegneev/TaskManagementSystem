using TaskManagementSystem.DataAccess;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace TaskManagementSystem.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management System Api", Version = "v1" });
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                c.IncludeXmlComments(xmlFullPath);
            });
            services.AddDataAccessLayer(configuration);
            services.AddCors(x => x.AddPolicy("Frontend",
                x =>
                {
                    x.WithOrigins("http://localhost:3000");
                    x.AllowAnyMethod();
                    x.AllowAnyHeader();
                }));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("Frontend");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}