namespace FlightFinderApi.Models
{
    public class Price
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public double Adult { get; set; }
        public double Child { get; set; }
      //  public virtual Itinerary Itinerary { get; set; }

    }
}
