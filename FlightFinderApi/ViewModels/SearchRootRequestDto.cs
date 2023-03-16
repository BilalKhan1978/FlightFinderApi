namespace FlightFinderApi.ViewModels
{
    public class SearchRootRequestDto
    {
        public string? DepartureDestination { get; set; }
        public string? ArrivalDestination { get; set; }
        public string? DepartureTime { get; set; }
    }
}
