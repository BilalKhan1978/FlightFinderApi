using FlightFinderApi.ViewModels;

namespace FlightFinderApi.Services.Interfaces
{
    public interface IBookingService
    {
      Task AddBooking (AddBookingRequestDto addBookingRequestDto);
        Task<List<GetBookingRequestDto>> GetBookingByUserName(string userName);
    }
}
