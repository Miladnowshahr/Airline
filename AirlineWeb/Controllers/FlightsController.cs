using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Data;
using AirlineWeb.Dtos;
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

        public FlightsController(IMapper mapper, AirlineDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
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

            _mapper.Map(flightDetailUpdateDto,flight);
            _dbContext.SaveChanges();
            return NoContent();

        }


    }
}