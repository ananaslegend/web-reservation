using System;
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
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<WebReservationContext>();
                
            // if (!context.reservations.Any())
            // {
            //     for (var i = 1; i < 17; i++)
            //     {
            //         context.reservations.Add(
            //             new reservation("Daniil", "380-63-788-83-90", new DateTime(2021, 12, 12, 12, 0, 0), 
            //                 12, i, 1, "test that need to rm", 1));
            //         context.SaveChanges();
            //     }
            // }
        }

        
    }
}