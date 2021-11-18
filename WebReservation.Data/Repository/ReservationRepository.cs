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
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly WebReservationContext context;
        public ReservationRepository(WebReservationContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<Reservation> All 
            => context.Reservations.ToList();

        public void Add(Reservation entity) 
            => context.Reservations.Add(entity);

        public void Delete(Reservation entity)
        {
            context.Reservations.Remove(entity);
            context.SaveChanges();
        }

        public void Update(Reservation entity)
        {
            context.Reservations.Update(entity);
            context.SaveChanges();
        }

        public Reservation FindById(int Id)
            => context.Reservations.FirstOrDefault(e => e.id == Id);
        
        // todo доделать
        public Reservation FindByName(string guestName)
            => context.Reservations.FirstOrDefault(guest => guest.GuestName == guestName);

        
        // todo доделать
        public Reservation FindByDate(DateTime dateTime)
            => context.Reservations.FirstOrDefault(date => date.ReservationDate == dateTime);
        
        // todo можно сделать лучше
        private List<Reservation> FindAllDayReservations(DateTime dateTime)
            => context.Reservations.ToList().Where(reservation => 
                reservation.ReservationDate.ToString().StartsWith($"{dateTime.ToShortDateString()}")).ToList();

        private bool BetweenRange(DateTime x, DateTime min, DateTime max)
            => ((x > min) & (x < max));

        // todo добавить еще метод, чтобы можно было расширить функционал
        private bool IsItFreeTime(int hall, DateTime dateTime, int hours,  List<Reservation> AllDayReservations, int guestNumber)
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
                if ((((reservation.NumTable >= min) & (reservation.NumTable <= max)) & (reservation.hall == hall) & (
                    BetweenRange(reservation.ReservationDate, dateTime, dateTime.AddHours(hours)) ^
                    BetweenRange(reservation.EndTimeDate, dateTime, dateTime.AddHours(hours)) ^
                    BetweenRange(dateTime, reservation.ReservationDate, reservation.EndTimeDate))))
                        
                    list[index] = false;
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

        public int AddReservation(string guestName, string phoneNumber, int year, int month, int day, int dayHours, 
            int minutes, int hours, int numTable, int hall, string guestComment, int guestNumber)
        {
            Reservation reservation = new(guestName, phoneNumber, year, month, day, dayHours, minutes,
                hours, numTable, hall,guestComment, guestNumber);
            
            context.Reservations.Add(reservation);
            context.SaveChanges();
        
            return reservation.id;
        }
    }
}