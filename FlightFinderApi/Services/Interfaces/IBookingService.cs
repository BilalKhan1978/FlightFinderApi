using FlightFinderApi.ViewModels;

namespace FlightFinderApi.Services.Interfaces
{
    public interface IBookingService
    {
      Task AddBooking (List<AddBookingRequestDto> addBookingRequestDto);
      Task<List<GetBookingRequestDto>> GetBookingByUserName(string userName);
      Task DeleteBooking(string bookingReference);
    }
}
