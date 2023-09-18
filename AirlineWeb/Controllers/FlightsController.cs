using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AirlineDbContext _dbContext;
        private readonly IMessageBusClient _messageBus;
        public FlightsController(IMapper mapper, AirlineDbContext dbContext, IMessageBusClient messageBus)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _messageBus = messageBus;
        }

        public ActionResult<FlightDetailReadDto> GetFlightDetailsByCode(string flightCode){
            
            var flight = _dbContext.FlightDetails.FirstOrDefault(f=>f.FlightCode == flightCode);

            if(flight == null)
                return NotFound();

            return Ok(_mapper.Map<FlightDetailReadDto>(flight));

        }

        [HttpPost]
        public ActionResult<FlightDetailReadDto> CreateFlight(FlightDetailCreateDto flightDetailCreateDto){

            var flight = _dbContext.FlightDetails.FirstOrDefault(f=>f.FlightCode == flightDetailCreateDto.FlightCode);
            if(flight == null)
            {
                var flightDetailModel = _mapper.Map<FlightDetail>(flightDetailCreateDto);
                try{
                    _dbContext.FlightDetails.Add(flightDetailModel);
                    _dbContext.SaveChanges();
                }
                catch(Exception ex){
                    return BadRequest(ex.ToString()); 
                }
                var flightDetailReadDto = _mapper.Map<FlightDetailReadDto>(flightDetailModel);
                return CreatedAtRoute(nameof(GetFlightDetailsByCode),new{flightCode = flightDetailReadDto.FlightCode},flight);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateFlightDetail(int id, FlightDetailUpdateDto flightDetailUpdateDto){
            var flight = _dbContext.FlightDetails.FirstOrDefault(f=>f.Id == id);

            if(flight is null)
                return NotFound();

            decimal oldPrice = flight.Price;

            _mapper.Map(flightDetailUpdateDto,flight);
            
            try
            {
                _dbContext.SaveChanges();
                if (oldPrice !=flight.Price)
                {
                    Console.WriteLine("Price Changed - Place message on bus");

                    var message = new NotificationMessageDto
                    {
                        WebhookType = "pricechange",
                        OldPrice = oldPrice,
                        NewPrice = flight.Price,
                        FlightCode = flight.FlightCode,

                    };

                    _messageBus.SendMessage(message);

                }
                else
                {
                    Console.WriteLine("No Price change");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();

        }


    }
}