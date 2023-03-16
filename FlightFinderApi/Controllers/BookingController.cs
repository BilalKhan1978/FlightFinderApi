using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] List<AddBookingRequestDto> addBookingRequestDto)
        {
            try
            {
                await _bookingService.AddBooking(addBookingRequestDto);
                return Ok("New Booking has been added");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetBookingByUserName([FromRoute] string userName)
        {
            var result = await _bookingService.GetBookingByUserName(userName);
            return Ok(result);
        }

        [HttpDelete("{bookingReference}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] string bookingReference)
        {
            try
            {   
              await _bookingService.DeleteBooking(bookingReference);
              return Ok("Desired booking has been cancelled");  
            }
            catch(Exception e)
            {
                if (e.Message.Contains("No such booking available against this booking reference"))
                    return Ok(" There is no booking against this booking reference"); 
                throw new Exception(e.Message);
            }
        }
    }
}
