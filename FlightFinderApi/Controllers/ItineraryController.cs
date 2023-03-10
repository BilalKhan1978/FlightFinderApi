using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _tineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _tineraryService = itineraryService;
        }

        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetItineraryByFlightId([FromRoute] string flightId)
        {
           try
           { 
            return Ok(await _tineraryService.GetItineraryByFlightId(flightId));
           }
           catch (Exception e)
           {
                if (e.Message.Contains("No such itinerary found"))
                    return NotFound("No such itinerary found");
                throw new Exception(e.Message);
           }
            
            }


    }
}
