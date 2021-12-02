using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WebReservation.Data.Models;

namespace WebReservation.Data.Context
{
    public class WebReservationContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; } 

        public WebReservationContext(DbContextOptions<WebReservationContext> options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;Port=5432;Database=postgres;User Id=webadmin;Password=password;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}