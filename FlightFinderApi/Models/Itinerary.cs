
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightFinderApi.Models
{
    public class Itinerary
    {
        [Key]        
        public string Flight_Id { get; set; }
        public DateTime DepartureAt { get; set; }
        public DateTime ArrivalAt { get; set; }
        public int AvailableSeats { get; set; }
        public Price Prices { get; set; }
        public virtual Root Root { get; set; }

    }
}
