using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebReservation.Data.Context;
using WebReservation.Data.Models;

namespace WebReservation.Data.Repository
{
    public class ReservationRepository : IRepository<reservations>
    {
        private readonly WebReservationContext context;
        public ReservationRepository(WebReservationContext _context)
        {
            context = _context;
        }

        public IEnumerable<Model> All()
        {
            var reservationModels = context.reservations.Join(
                context.guests, 
                r => r.guest_id,
                g => g.id,
                (r, g) => new Model
                {
                    Id = r.id,
                    GuestId = g.id,
                    GuestName = g.name, 
                    PhoneNumber = g.phone_number, 
                    GuestsNumber = g.number,
                    Comment = g.comment, 
                    DateStart = r.date_start_time,
                    DateEnd = r.date_end_time, 
                    Hall = r.hall,
                    Table = r.num_table, 
                    Hours = r.hours
                }).AsEnumerable();
            
            return reservationModels;
        }

        public void Delete(int id)
        {
            var reservation = context.reservations.First(i => i.id == id);
            context.Remove(reservation);
            context.SaveChanges();
            
            var guest = context.guests.First(i => i.id == reservation.guest_id);
            context.Remove(guest);
            context.SaveChanges();
            
        }
        
        public Model FindById(int Id)
        {
            var reservation = context.reservations.First(i => i.id == Id);
            var guest = context.guests.First(i => i.id == reservation.guest_id);
            
            return Model.ToModel(reservation, guest);
        }
        
        public List<Model> FindAllDayReservations(DateTime dateTime)
            => All().Where(r => r.DateStart.ToString().StartsWith($"{dateTime.ToShortDateString()}")).ToList();

        private static bool BetweenRange(DateTime x, DateTime min, DateTime max)
            => ((x > min) & (x < max));
        
        private static bool IsItFreeTime(int hall, DateTime dateTime, int hours, IReadOnlyList<Model> AllDayReservations, int guestNumber)
        {
            int min = 0, max = 0;
            List<bool> list = new();
        
            if(hall == 1 & guestNumber > 2)
            {
                min = 9; max = 16;
                
                for(var i = 0; i < 8; ++i) 
                    list.Add(true);
            }
            else if(hall == 1 & guestNumber <= 2)
            {
                min = 1; max = 16;
                
                for(var i = 0; i < 16; ++i) 
                    list.Add(true);
            }
            else if (hall == 2)
            {
                min = 17; max = 21;
                
                for(var i = 0; i < 5; ++i) 
                    list.Add(true);
            }
        
            for (var index = 0; index < AllDayReservations.Count; index++)
            {
                var model = AllDayReservations[index];
                if ((((model.Table >= min) & (model.Table <= max)) & (model.Hall == hall) & (
                        BetweenRange(model.DateStart, dateTime, dateTime.AddHours(hours)) ^
                        BetweenRange(model.DateEnd, dateTime, dateTime.AddHours(hours)) ^
                        BetweenRange(dateTime, model.DateStart, model.DateEnd))))
                {
                    list[index] = false;
                }
            }

            return list.Contains(true);
        }
        
        public List<bool> FindFreeTables(int hall, int year, int month, int day, int dayHours, int minutes, int hours, int guestNumber)
        {
            var AllDayReservations = FindAllDayReservations(new DateTime(year, month, day));
            DateTime dateTime = new(year, month, day, dayHours, minutes, 0);
            List<bool> result = new()
            {
                IsItFreeTime(hall, dateTime.Subtract(TimeSpan.FromMinutes(30)), hours, AllDayReservations, guestNumber),
                IsItFreeTime(hall, dateTime.Subtract(TimeSpan.FromMinutes(15)), hours, AllDayReservations, guestNumber),
                IsItFreeTime(hall, dateTime, hours, AllDayReservations, guestNumber),
                IsItFreeTime(hall, dateTime.Add(TimeSpan.FromMinutes(15)), hours, AllDayReservations, guestNumber),
                IsItFreeTime(hall, dateTime.Add(TimeSpan.FromMinutes(30)), hours, AllDayReservations, guestNumber)
            };
        
            return result;
        }
        
        public int Add(Model model)
        {
            var guest = Model.ToGuest(model);
            context.guests.Add(guest);
            context.SaveChanges();
            
            var reservation = Model.ToReservation(model);
            reservation.guest_id = guest.id;
            context.reservations.Add(reservation);
            context.SaveChanges();

            return reservation.id;
        }
    }
}