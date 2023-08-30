using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private IMapper _mapper;
        public WebhookSubscriptionController(AirlineDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<WebhookSubscriptionReadDto> GetSubscriptionBySecret(string secret)
        {
            var subscription = _context.WebhookSubscriptions.FirstOrDefault(f=>f.Secret == secret);
            if(subscription == null){
                return NotFound();
            }

            return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
        }

        [HttpPost]
        public ActionResult<WebhookSubscriptionReadDto> CreateSubscription(WebhookSubscriptionCreateDto webhookSubscriptionCreateDto)
        {
            var subscription = _context.WebhookSubscriptions.FirstOrDefault(s => s.WebhookURI == webhookSubscriptionCreateDto.WebhookURI);

            if(subscription == null)
            {
                subscription = _mapper.Map<WebhookSubscription>(webhookSubscriptionCreateDto); 
                subscription.Secret = Guid.NewGuid().ToString();
                subscription.WebhookPublisher = "PanAus";
                
                try{
                    _context.WebhookSubscriptions.Add(subscription);
                    _context.SaveChanges();
                    
                }
                catch(Exception ex){
                    return BadRequest(ex.Message);
                }

                var  webhookSubscriptionReadDto = _mapper.Map<WebhookSubscriptionReadDto>(subscription);
                return CreatedAtRoute(nameof(GetSubscriptionBySecret),new {secret = webhookSubscriptionReadDto.Secret},webhookSubscriptionReadDto);
            }
            else{
                return NoContent();
            }
        }
    }
}