namespace FlightFinderApi.ViewModels
{
    public class GetRootsRequestDto
    {
        public string DepartureDestination { get; set; }  // From Root table
        public string ArrivalDestination { get; set; }    // From Root Table
        public GetItineraryRequestDto FlightRequests { get; set; }   



    }
}
