using FlightFinderApi.Models;
using FlightFinderApi.ViewModels;

namespace FlightFinderApi.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<GetItineraryRequestDto> GetItineraryByFlightId(string flightId);
    }
}
