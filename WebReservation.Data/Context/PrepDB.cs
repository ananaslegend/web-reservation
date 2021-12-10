using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebReservation.Data.Models;

namespace WebReservation.Data.Context
{
    public static class PrepDB
    { 
        public static void ApplyMigration(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<WebReservationContext>();
                context.Database.Migrate();
                if (!context.Reservations.Any())
                { 
                    context.Reservations.Add(
                        new Reservation("Daniil", "380-63-788-83-90", 2021, 12, 12, 12, 0, 
                            12, 1, 1, "test that need to rm", 1));
                    context.SaveChanges();
                }
            }
            
        }

        
    }
}