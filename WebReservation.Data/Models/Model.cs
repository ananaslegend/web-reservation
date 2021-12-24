using System;

namespace WebReservation.Data.Models
{
    public class Model
    {
        public Model() { }
        
        public int Id { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public int GuestsNumber { get; set; }
        public string Comment { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Hall { get; set; }
        public int Table { get; set; }
        public int Hours { get; set; }
        
        public static Model ToModel(reservations reservation, guests guest)
        {
            var model = new Model()
            {
                Id = reservation.id,
                GuestId = reservation.guest_id,
                GuestName = guest.name,
                PhoneNumber = guest.phone_number,
                GuestsNumber = guest.number,
                Comment = guest.comment,
                DateStart = reservation.date_start_time,
                DateEnd = reservation.date_end_time,
                Hall = reservation.hall,
                Table = reservation.num_table,
                Hours = reservation.hours
            };

            return model;
        }
        
        public static reservations ToReservation(Model model)
        {
            var reservation = new reservations()
            {
                id = model.Id,
                date_start_time = model.DateStart,
                date_end_time = model.DateEnd,
                hall = model.Hall,
                num_table = model.Table,
                hours = model.Hours
            };
            
            return reservation;
        }
        
        public static guests ToGuest(Model model)
        {
            var guest = new guests()
            {
                id = model.GuestId,
                name = model.GuestName,
                phone_number = model.PhoneNumber,
                comment = model.Comment,
                number = model.GuestsNumber
            };
            
            return guest;
        }
    }
}