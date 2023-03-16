namespace FlightFinderApi.Models
{
    public class Booking
    {
      public int Id { get; set; }
      public User Users { get; set; }
      public Itinerary Itineraries { get; set; }
      public int TotalAdultSeats { get; set; }
      public int TotalChildSeats { get; set; }
      public string BookingReference { get; set; }
    }
}
