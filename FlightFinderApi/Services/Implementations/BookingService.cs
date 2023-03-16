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

        public async Task AddBooking(List<AddBookingRequestDto> addBookingRequestDto)
        {
            var randomNumber = GetRandomAlphaNumeric();
            List<Booking> bookingList = new List<Booking>();
            foreach (var item in addBookingRequestDto)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);
                if (user == null) throw new Exception("No user found against this user-Id");

                var flight = await _dbContext.Itineraries.FirstOrDefaultAsync(x => x.Flight_Id == item.Flight_Id);
                if (flight == null) throw new Exception("No flight Info found against this flight-Id");
                if (flight.AvailableSeats < (item.TotalAdultSeats + item.TotalChildSeats))
                    throw new Exception("Desired no. of seats are not available");
                flight.AvailableSeats = flight.AvailableSeats - (item.TotalChildSeats + item.TotalAdultSeats);

                bookingList.Add(new Booking()
                {
                    Users = user,
                    Itineraries = flight,
                    TotalAdultSeats = item.TotalAdultSeats,
                    TotalChildSeats = item.TotalChildSeats,
                    BookingReference = randomNumber
                });
                _dbContext.Update(flight);
            }
            await _dbContext.AddRangeAsync(bookingList);
            await _dbContext.SaveChangesAsync();
        }

        public static string GetRandomAlphaNumeric()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(6).ToArray());
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
                    BookingReference = booking.BookingReference,
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

        public async Task DeleteBooking(string bookingReference)
        {
            var booking = await _dbContext.Bookings.Include(x => x.Itineraries).Where(x => x.BookingReference == bookingReference).ToListAsync();
            if (booking.Count < 1) throw new Exception("No such booking available against this booking reference");
            _dbContext.RemoveRange(booking);
           
            foreach(var item in booking)
            {
                var flight = await _dbContext.Itineraries.FirstOrDefaultAsync(x => x.Flight_Id == item.Itineraries.Flight_Id);
                if (flight != null)
                {
                    flight.AvailableSeats += item.TotalAdultSeats + item.TotalChildSeats;
                    _dbContext.Itineraries.Update(flight);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
