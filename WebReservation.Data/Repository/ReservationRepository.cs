using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebReservation.Data.Context;
using WebReservation.Data.Models;

namespace WebReservation.Data.Repository
{
    public class ReservationRepository : IRepository<reservation>
    {
        private readonly WebReservationContext context;
        public ReservationRepository(WebReservationContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<reservation> All 
            => context.reservations.ToList();

        public void Add(reservation entity) 
            => context.reservations.Add(entity);

        public void Delete(reservation entity)
        {
            context.reservations.Remove(entity);
            context.SaveChanges();
        }

        public void Update(reservation entity)
        {
            context.reservations.Update(entity);
            context.SaveChanges();
        }

        public reservation FindById(int Id) 
            => context.reservations.Find(Id);
        
        // todo доделать
        public reservation FindByName(string guestName)
            => context.reservations.FirstOrDefault(guest => guest.guest_name == guestName);

        
        // todo rm
        public reservation FindByDate(DateTime dateTime)
            => context.reservations.FirstOrDefault(date => date.reservation_date == dateTime);
        
        // todo можно сделать лучше
        public List<reservation> FindAllDayReservations(DateTime dateTime)
            => context.reservations.ToList().Where(reservation => 
                reservation.reservation_date.ToString().StartsWith($"{dateTime.ToShortDateString()}")).ToList();

        private static bool BetweenRange(DateTime x, DateTime min, DateTime max)
            => ((x > min) & (x < max));

        // todo добавить еще метод, чтобы можно было расширить функционал
        private static bool IsItFreeTime(int hall, DateTime dateTime, int hours, IReadOnlyList<reservation> AllDayReservations, int guestNumber)
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
                var reservation = AllDayReservations[index];
                if ((((reservation.num_table >= min) & (reservation.num_table <= max)) & (reservation.hall == hall) & (
                        BetweenRange(reservation.reservation_date, dateTime, dateTime.AddHours(hours)) ^
                        BetweenRange(reservation.end_time_date, dateTime, dateTime.AddHours(hours)) ^
                        BetweenRange(dateTime, reservation.reservation_date, reservation.end_time_date))))
                {
                    list[index] = false;
                }
            }

            for (var index = 0; index < list.Count; index++)
            {
                var VARIABLE = list[index];
                Console.WriteLine(index+1 + " - " + VARIABLE);
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

        public int AddReservation(reservation _reservation)
        {
            context.reservations.Add(_reservation);
            context.SaveChanges();
        
            return _reservation.id;
        }
    }
}