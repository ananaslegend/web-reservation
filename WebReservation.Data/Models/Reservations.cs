using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebReservation.Data.Models
{
    public class reservations
    {
        public reservations() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int guest_id { get; set; }
        public DateTime date_start_time { get; set; }
        public DateTime date_end_time { get; set; }
        public int hall { get; set; }
        public int num_table { get; set; }
        public int hours { get; set; }
    }
}