using FlightFinderApi.Models;

namespace FlightFinderApi.ViewModels
{
    public class AddBookingRequestDto
    {
        public int UserId { get; set; }
        public string Flight_Id { get; set; }
        public int TotalAdultSeats { get; set; }
        public int TotalChildSeats { get; set; }
    }
}
