using TaskManagementSystem.DataAccess;
using AutoMapper;
using System.Reflection;

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
            services.AddSwaggerGen();
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