using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebReservation.Data.Context;
using WebReservation.Data.Models;
using WebReservation.Data.Repository;

namespace WebReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReservationController : ControllerBase
    {
        private readonly ReservationRepository reservationContext;

        public ReservationController(IRepository<Reservation> reservationContext)
            => this.reservationContext = (ReservationRepository)reservationContext;
        
        [HttpGet("hello")] 
        public string Hello() 
            => "Hello world!";
        
        [HttpGet] 
        public IEnumerable<Reservation> Get() 
            => reservationContext.All;

        [HttpGet("id/{int id}")] 
        public ActionResult<Reservation> Get(int id) 
            => reservationContext.FindById(id);

        [HttpGet("name/{guestName}")]
        public ActionResult<Reservation> Get(string guestName) 
            => reservationContext.FindByName(guestName);

        [HttpDelete("remove/{guestName}")]
        public void Delete(string guestName)
            => reservationContext.Delete(reservationContext.FindByName(guestName));

        [HttpGet("find-by-date/{year},{month},{day},{hours},{minutes}")]
        public Reservation FindByDate(int year, int month, int day, int hours, int minutes)
            => reservationContext.FindByDate(new DateTime(year, month, day, hours, minutes, 0)); //2021, 6, 4, 22, 0
        
        [HttpGet("find-by-date/{year},{month},{day}")]
        public Reservation FindByDate(int year, int month, int day)
            => reservationContext.FindByDate(new DateTime(year, month, day));

        [HttpGet("find-free-tables/{hall},{year},{month},{day},{dayHours},{minutes},{hours},{guestNumber}")]
        public IEnumerable<bool> FindFreeTables(int hall, int year, int month, int day, int dayHours, int minutes, int hours, int guestNumber)
            => reservationContext.FindFreeTables(hall, year, month, day, dayHours, minutes, hours, guestNumber);

        [HttpPost("add-resevation/{guestName},{phoneNumber},{year},{month},{day},{dayHours},{minutes},{hours}," +
                  "{numTable}, {hall},{guestComment},{guestNumber}")]
        public int AddReservation(string guestName, string phoneNumber, int year, int month, int day, int dayHours,
            int minutes, int hours, int numTable, int hall, string guestComment, int guestNumber)
            => reservationContext.AddReservation(guestName, phoneNumber, year, month, day, dayHours, minutes,
                hours, numTable, hall, guestComment, guestNumber);

    }
}