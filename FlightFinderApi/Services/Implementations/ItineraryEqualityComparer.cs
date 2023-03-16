using FlightFinderApi.Models;

namespace FlightFinderApi.Services.Implementations
{
    public class ItineraryEqualityComparer : IEqualityComparer<Itinerary>
    {
        
            public int GetHashCode(Itinerary itinerary) { return itinerary.Flight_Id.GetHashCode(); }
            public bool Equals(Itinerary itinerary1, Itinerary itinerary2) { return itinerary1.Flight_Id == itinerary2.Flight_Id; }
        
    }
}
