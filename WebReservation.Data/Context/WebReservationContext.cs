using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebReservation.Data.Models;

namespace WebReservation.Data.Context
{
    public class WebReservationContext : DbContext
    {
        public DbSet<reservation> reservations { get; set; } 

        public WebReservationContext(DbContextOptions<WebReservationContext> options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}