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

        [HttpGet("all")]
        public ActionResult<IEnumerable<Reservation>> Get()
        {
            if (!reservationContext.All.Any()) 
                return NotFound();
            
            return Ok(reservationContext.All);
        }

        [HttpGet("id/{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            var reservation = reservationContext.FindById(id);
            if (reservation == null)
                return NotFound();
            return reservation;
        }

        [HttpDelete("remove/{id}")]
        public ActionResult Delete(int id)
        {
            var reservation = reservationContext.FindById(id);
            if (reservation == null)
                return NotFound();
            reservationContext.Delete(reservation);
            return Ok();
        }

        [HttpGet("find-by-date/{year},{month},{day},{hours},{minutes}")]
        public ActionResult<List<Reservation>> FindByDate(int year, int month, int day, int hours, int minutes)
            => Ok(reservationContext.FindAllDayReservations(new DateTime(year, month, day, hours, minutes, 0)));

        [HttpGet("find-by-date/{year},{month},{day}")]
        public ActionResult<List<Reservation>> FindByDate(int year, int month, int day)
            => Ok(reservationContext.FindAllDayReservations(new DateTime(year, month, day)));

        [HttpGet("find-free-tables/{hall},{year},{month},{day},{dayHours},{minutes},{hours},{guestNumber}")]
        public ActionResult<IEnumerable<bool>> FindFreeTables(int hall, int year, int month, int day, int dayHours, int minutes, int hours, int guestNumber)
            => reservationContext.FindFreeTables(hall, year, month, day, dayHours, minutes, hours, guestNumber);

        // [HttpPost("add-resevation/{guestName},{phoneNumber},{year},{month},{day},{dayHours},{minutes},{hours}," +
        //           "{numTable}, {hall},{guestComment},{guestNumber}")]
        // public ActionResult<int> AddReservation(string guestName, string phoneNumber, int year, int month, int day, int dayHours,
        //     int minutes, int hours, int numTable, int hall, string guestComment, int guestNumber)
        //     => reservationContext.AddReservation(guestName, phoneNumber, year, month, day, dayHours, minutes,
        //         hours, numTable, hall, guestComment, guestNumber);
        
        [HttpPost("add-resevation/{guestName},{phoneNumber},{year},{month},{day},{dayHours},{minutes},{hours}," +
                  "{numTable}, {hall},{guestComment},{guestNumber}")]
        public ActionResult<int> AddReservation(string guestName, string phoneNumber, int year, int month, int day, int dayHours,
            int minutes, int hours, int numTable, int hall, string guestComment, int guestNumber)
            => reservationContext.AddReservation(guestName, phoneNumber, year, month, day, dayHours, minutes,
                hours, numTable, hall, guestComment, guestNumber);

    }
}