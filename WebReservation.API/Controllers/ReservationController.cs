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

        public ReservationController(IRepository<reservations> reservationContext)
            => this.reservationContext = (ReservationRepository)reservationContext;
        
        /// <summary>
        /// Gat one reservation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One reservation</returns>
        /// <response code="200">Success request</response>
        /// <response code="404">Reservation with this id does not exist</response>
        /// <response code="400">Input value not valid</response>
        [HttpGet("/id")]
        public ActionResult<Model> Get([FromQuery] int id)
        {
            try
            {
                var reservationModel = reservationContext.FindById(id);
                return reservationModel;
            }
            catch
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Delete one reservation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One reservation</returns>
        /// <response code="200">Success request</response>
        /// <response code="404">Reservation with this id does not exist</response>
        /// <response code="400">Input value not valid</response>
        [HttpDelete("/id")]
        public ActionResult Delete([FromQuery] int id)
        {
            try
            {
                reservationContext.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        
    // [HttpGet("find-by-date/{year:int},{month:int},{day:int}")]
    // public ActionResult<List<Model>> FindByDate(int year, int month, int day)
    //     => Ok(reservationContext.FindAllDayReservations(new DateTime(year, month, day)));

    /// <summary>
    /// Adding reservation json object  
    /// </summary>
    /// <param name="reservation"></param>
    /// <returns>id of reservation</returns>
    /// <response code="200">Success request</response>
    /// <response code="400">Most likely problems with the type of one of the JSON value</response>
    [HttpPost]
        public ActionResult<int> AddReservation([FromBody] Model model)
            => reservationContext.AddReservation(model);

    /// <summary>
    /// Finds an order by the selected hall, time, number of guests   
    /// </summary>
    /// <param name="hall,year,month,day,dayHours,minutes,hours,guestNumber"></param>
    /// <returns>List of 5 bool values denoting time -30 -15 [selected time] +15 +30</returns>
    /// <response code="200">Success request</response>
    [HttpGet("find-free-tables")]
    public ActionResult<IEnumerable<bool>> FindFreeTables(
        [FromQuery]int hall, 
        [FromQuery]int year, 
        [FromQuery]int month, 
        [FromQuery]int day,
        [FromQuery]int dayHours, 
        [FromQuery]int minutes, 
        [FromQuery]int hours, 
        [FromQuery]int guestNumber)
        => reservationContext.FindFreeTables(hall, year, month, day, dayHours, minutes, hours, guestNumber);
    
    /// <summary>
    /// Get all reservation in database
    /// </summary>
    /// <returns>All reservation</returns>
    /// <response code="200">Success request</response>
    /// <response code="404">Database is empty</response>
    [HttpGet("/all-reservations")]
    public ActionResult<IEnumerable<Model>> Get()
    {
        if (!reservationContext.All().Any()) 
            return NotFound();
            
        return Ok(reservationContext.All());
            
    }
    }
}