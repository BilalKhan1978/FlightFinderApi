using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightFinderApi.Models
{
    public class Root
    {
        [Key]        
        public string Route_Id { get; set; }
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public List<Itinerary> Itineraries { get; set; }
    }
}
