using AutoMapper;
using FlightFinderApi.Data;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightFinderApi.Services.Implementations
{
    public class ItineraryService : IItineraryService
    {
        // Inject the services
        public readonly FlightFinderDbContext _dbContext;
        IMapper _mapper;
        public ItineraryService(FlightFinderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetItineraryRequestDto> GetItineraryByFlightId(string flightId)
        {
            var itinerary = await _dbContext.Itineraries.Include(x=>x.Prices).
                FirstOrDefaultAsync(x => x.Flight_Id == flightId);
            if (itinerary == null) throw new Exception("No such itinerary found");
            var itineraryDto = _mapper.Map<GetItineraryRequestDto>(itinerary);
            return itineraryDto;

            // other way without mapper
            //GetItineraryRequestDto result = new GetItineraryRequestDto()
            //{
            //    Flight_Id = itinerary.Flight_Id,
            //    ArrivalAt = itinerary.ArrivalAt,
            //    DepartureAt = itinerary.DepartureAt,
            //    Adult = itinerary.Prices.Adult,
            //    Child = itinerary.Prices.Child,
            //    Currency = itinerary.Prices.Currency
            //};
        }

        
    }
}
