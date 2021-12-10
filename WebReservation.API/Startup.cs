using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebReservation.Data.Context;
using WebReservation.Data.Models;
using WebReservation.Data.Repository;


namespace WebReservation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Controllers
            services.AddControllers();

            // Add DB connection, Migrations 
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<WebReservationContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("WebReservation.Data")));

            // Add Repo pattern for Reservation
            services.AddScoped<IRepository<Reservation>, ReservationRepository>();

            // Add Swagger
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Web Reservation", Version = "v1"}));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            PrepDB.ApplyMigration(app);
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Reservation V1"));
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}