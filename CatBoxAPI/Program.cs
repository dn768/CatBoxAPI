
using CatBoxAPI.DB;
using CatBoxAPI.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace CatBoxAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<CatBoxContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("CatBoxDatabase")));

            builder.Services.AddScoped<ICatProfileService, CatProfileService>();
            builder.Services.AddScoped<IBoxRegistrationService, BoxRegistrationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
