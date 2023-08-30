using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Dtos
{
    public class FlightDetailUpdateDto
    {
        [Required]
        public int Id{get;set;}
        
        public string FlightCode{get;set;}
        
        public decimal Price{get;set;}
    }
}