namespace FlightFinderApi.ViewModels
{
    public class GetItineraryRequestDto
    {
        public string Flight_Id { get; set; }   // From Itinerary table
        public string DepartureAt { get; set; } // From Itinerary table
        public string ArrivalAt { get; set; }  // From Itinerary table
        public int AvailabeSeats { get; set; }
        public string Currency { get; set; }
        public double Adult { get; set; }    //From Price table
        public double Child { get; set; }    //From Price table
        
    }
}
