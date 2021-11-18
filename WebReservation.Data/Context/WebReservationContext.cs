using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WebReservation.Data.Models;

namespace WebReservation.Data.Context
{
    public class WebReservationContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; } // reservation 

        public WebReservationContext(DbContextOptions<WebReservationContext> options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}