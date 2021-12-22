using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebReservation.Data.Models
{
    public class reservation
    {
        public reservation() { }
        
        // todo доделать
        public reservation(string guestName, string phoneNumber, DateTime dateTime, int hours, int numTable, int hall, string guestComment, int guestNumber)
        {
            guest_name = guestName;
            phone_number = phoneNumber;
            reservation_date = dateTime;
            this.hours = hours;
            num_table = numTable;
            this.hall = hall;
            guest_comment = guestComment;
            end_time_date = reservation_date + new TimeSpan(hours, 0, 0);
            guest_number = guestNumber;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string guest_name { get; set; }
        public string phone_number { get; set; }
        public DateTime reservation_date { get; set; }
        public int hours { get; set; }
        public int num_table { get; set; }
        public int hall { get; set; }
        public string guest_comment { get; set; }
        public DateTime end_time_date { get; set; }
        public int guest_number { get; set; }
    }
}