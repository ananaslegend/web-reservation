using System.ComponentModel.DataAnnotations.Schema;

namespace WebReservation.Data.Models
{
    public class guests
    {
        public guests() { }
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string comment { get; set; }
        public int number { get; set; }
    }
}