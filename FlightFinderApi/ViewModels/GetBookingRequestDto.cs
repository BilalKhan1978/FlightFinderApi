namespace FlightFinderApi.ViewModels
{
    public class GetBookingRequestDto
    {

        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public int TotalAdultSeats { get; set; }
        public int TotalChildSeats { get; set; }
        public string Flight_Id { get; set; }
        public DateTime DepartureAt { get; set; }
        public DateTime ArrivalAt { get; set; }
        public string Currency { get; set; }
        public double PriceForAdult { get; set; }
        public double PriceForChild { get; set; }

    }
}
