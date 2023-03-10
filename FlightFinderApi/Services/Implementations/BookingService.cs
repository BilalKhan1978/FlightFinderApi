using AutoMapper;
using FlightFinderApi.Data;
using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightFinderApi.Services.Implementations
{
    public class BookingService : IBookingService
    {
      private readonly FlightFinderDbContext _dbContext;
      IMapper _mapper;
        public BookingService(FlightFinderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task AddBooking(AddBookingRequestDto addBookingRequestDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == addBookingRequestDto.UserId);
            if (user == null) throw new Exception("No user found against this user-Id");

            var flight = await _dbContext.Itineraries.FirstOrDefaultAsync(x => x.Flight_Id == addBookingRequestDto.Flight_Id);
                if (flight == null) throw new Exception("No flight Info found against this flight-Id");
            if (flight.AvailableSeats < (addBookingRequestDto.TotalAdultSeats + addBookingRequestDto.TotalChildSeats)) 
                throw new Exception("Desired no. of seats are not available");
            flight.AvailableSeats = flight.AvailableSeats - (addBookingRequestDto.TotalChildSeats + addBookingRequestDto.TotalAdultSeats);
            
            Booking booking = new Booking()
            {
                Users = user,
                Itineraries = flight,
                TotalAdultSeats = addBookingRequestDto.TotalAdultSeats,
                TotalChildSeats = addBookingRequestDto.TotalChildSeats
            };
                  _dbContext.Update(flight);
            await _dbContext.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GetBookingRequestDto>> GetBookingByUserName(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) throw new Exception("notfound");
            var bookingList = await _dbContext.Bookings.Include(x => x.Users).Include(x => x.Itineraries).ThenInclude(x=>x.Prices).Where(x => x.Users.Id == user.Id).ToListAsync();


            List<GetBookingRequestDto> getBookingRequestDto = new List<GetBookingRequestDto>();
            foreach (var booking in bookingList )
            {
                var root = await GetRoot(booking.Itineraries);
                getBookingRequestDto.Add(new GetBookingRequestDto()
                {
                  DepartureDestination = root.DepartureDestination,
                    ArrivalDestination = root.ArrivalDestination,
                    TotalAdultSeats = booking.TotalAdultSeats,
                    TotalChildSeats = booking.TotalChildSeats,
                    Flight_Id = booking.Itineraries.Flight_Id,
                    DepartureAt = booking.Itineraries.DepartureAt,
                    ArrivalAt = booking.Itineraries.ArrivalAt,
                    Currency = booking.Itineraries.Prices.Currency,
                    PriceForAdult = booking.Itineraries.Prices.Adult,
                    PriceForChild = booking.Itineraries.Prices.Child
                });
            };
            return getBookingRequestDto;
        }
        private async Task<Root> GetRoot(Itinerary flight)
        {
            return await _dbContext.Roots.Include(x => x.Itineraries).FirstOrDefaultAsync(x => x.Itineraries.Contains(flight));
              //  _dbContext.Itineraries.Include(x => x.Root).FirstOrDefaultAsync(x => x.Flight_Id == flight_id);
        }
    }
}
