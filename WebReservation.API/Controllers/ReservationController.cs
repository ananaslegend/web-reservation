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

        
        /// <summary>
        /// Get all reservation in database
        /// </summary>
        /// <returns>All reservation</returns>
        /// <response code="200">Success request</response>
        /// <response code="404">Database is empty</response>
        [HttpGet("all")]
        public ActionResult<IEnumerable<Reservation>> Get()
        {
            if (!reservationContext.All.Any()) 
                return NotFound();
            
            return Ok(reservationContext.All);
        }

        /// <summary>
        /// Gat one reservation via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One reservation</returns>
        /// <response code="200">Success request</response>
        /// <response code="404">Reservation with this id does not exist</response>
        [HttpGet("id")]
        public ActionResult<Reservation> Get([FromQuery] int id)
        {
            var reservation = reservationContext.FindById(id);
            if (reservation == null)
                return NotFound();
            return reservation;
        }

        /// <summary>
        /// Delete one reservation via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One reservation</returns>
        /// <response code="200">Success request</response>
        /// <response code="404">Reservation with this id does not exist</response>
        [HttpDelete("remove")]
        public ActionResult Delete([FromQuery] int id)
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
        
/// <summary>
/// Adding reservation / Need an json object веталь, ты сам все знаешь че я тут распираюсь) 
/// </summary>
/// <param name="reservation"></param>
/// <returns>id of reservation</returns>
        [HttpPost("add-reservation")]
        public ActionResult<int> AddReservation([FromBody] Reservation reservation)
            => reservationContext.AddReservation(reservation);

    }
}