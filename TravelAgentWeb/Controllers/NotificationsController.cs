using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.Data;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDbContext _context;

        public NotificationsController(TravelAgentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult FlightChanged(FlightDetailUpdateDto flightDetailUpdateDto)
        {
            var secretModel = _context.SubscriptionSecret.FirstOrDefault(f=>f.Publisher == flightDetailUpdateDto.Publisher && f.Secret == flightDetailUpdateDto.Secret);
            if (secretModel == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Secret - Igonre Webhook!");
                Console.ResetColor();
                return Ok();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid Webhook!");
                Console.WriteLine($"Old Price {flightDetailUpdateDto.OldPrice}, New Price {flightDetailUpdateDto.NewPrice}");
                Console.ResetColor();
                return Ok();
            }
        }
    }
}