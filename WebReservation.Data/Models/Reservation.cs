using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebReservation.Data.Models
{
    public class Reservation
    {
        public Reservation() { }
        
        // todo доделать
        public Reservation(string guestName, string phoneNumber, int year, int month, int day, int dayHours, 
            int minutes, int hours, int numTable, int hall, string guestComment, int guestNumber)
        {
            GuestName = guestName;
            PhoneNumber = phoneNumber;
            ReservationDate = new DateTime(year, month, day, dayHours, minutes, 0);
            Hours = hours;
            NumTable = numTable;
            this.hall = hall;
            GuestComment = guestComment;
            EndTimeDate = ReservationDate + new TimeSpan(hours, 0, 0);
            GuestNumber = guestNumber;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public int Hours { get; set; }
        public int NumTable { get; set; }
        public int hall { get; set; }
        public string GuestComment { get; set; }
        public DateTime EndTimeDate { get; set; }
        public int GuestNumber { get; set; }
    }
}