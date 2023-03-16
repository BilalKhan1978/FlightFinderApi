namespace FlightFinderApi.ViewModels
{
    public class GetConnectedFlightRequestDto
    {
        public GetRootsRequestDto FirstRoot { get; set; }
        public GetRootsRequestDto SecondRoot { get; set; }
    }
}
