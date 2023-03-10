namespace FlightFinderApi.ViewModels
{
    public class GetItineraryRequestDto
    {
        public string Flight_Id { get; set; }   // From Itinerary table
        public DateTime DepartureAt { get; set; } // From Itinerary table
        public DateTime ArrivalAt { get; set; }  // From Itinerary table
        public string Currency { get; set; }
        public double Adult { get; set; }    //From Price table
        public double Child { get; set; }    //From Price table
        
    }
}
