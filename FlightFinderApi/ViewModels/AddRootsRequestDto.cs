using FlightFinderApi.Models;

namespace FlightFinderApi.ViewModels
{
    public class AddRootsRequestDto
    {
        public string Route_Id { get; set; }
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public List<Itinerary> Itineraries { get; set; }
    }
}
