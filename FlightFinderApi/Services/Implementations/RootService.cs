using AutoMapper;
using FlightFinderApi.Data;
using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightFinderApi.Services.Implementations
{
    public class RootService : IRootService
    {
        // Inject the services
        public readonly FlightFinderDbContext _dbContext;
        IMapper _mapper;
        public RootService(FlightFinderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        //public async Task AddRoots(List<Root> addRootsRequestDto)
        //{
        //    await _dbContext.AddRangeAsync(addRootsRequestDto);
        //    await _dbContext.SaveChangesAsync();
        //}
        public async Task<List<GetRootsRequestDto>> SearchRoots(SearchRootRequestDto searchRootRequestDto)
        {
           
            var roots = await _dbContext.Roots.Include(x => x.Itineraries).ThenInclude(x => x.Prices)
                            .Where(x => x.DepartureDestination == searchRootRequestDto.DepartureDestination &&
                                        x.ArrivalDestination == searchRootRequestDto.ArrivalDestination).ToListAsync();
            if (roots==null || roots.Count==0)
            {
                var connectedFlights = from f1 in _dbContext.Roots
                                       join f2 in _dbContext.Roots
                                       on f1.ArrivalDestination equals f2.DepartureDestination
                                       where f1.DepartureDestination == searchRootRequestDto.DepartureDestination && f2.ArrivalDestination == searchRootRequestDto.ArrivalDestination
                                       select new
                                       {
                                           Flight1 = f1.Itineraries,
                                           Origin1 = f1.DepartureDestination,
                                           Destination1 = f1.ArrivalDestination,
                                           Flight2 = f2.Itineraries,
                                           Origin2 = f2.DepartureDestination,
                                           Destination2 = f2.ArrivalDestination
                                       };
                var flights = connectedFlights.ToList();
                foreach (var flight in flights)
                {
                    roots = new List<Root>() { new Root()
                {
                    DepartureDestination=flight.Origin1,
                    ArrivalDestination=flight.Destination1,
                    Itineraries=flight.Flight1

                },
                new Root()
                {
                    DepartureDestination=flight.Origin2,
                    ArrivalDestination=flight.Destination2,
                    Itineraries=flight.Flight2

                }
                    };
                }
                                   

                
            }
                
            return await ConvertRootsToGetRootsRequestDto(roots);
        }

        private async Task<List<GetRootsRequestDto>> ConvertRootsToGetRootsRequestDto(List<Root> rootList)
        {
            List<GetRootsRequestDto> hello = new List<GetRootsRequestDto>();
            foreach (var root in rootList)
            {
                hello.Add(new GetRootsRequestDto()
                {
                    DepartureDestination = root.DepartureDestination,
                    ArrivalDestination = root.ArrivalDestination,   
                    FlightRequests = await ConvertItineraryToGetFlightRequestDto(root.Itineraries)

                });
            }
            return hello;
        }

        private async Task<List<GetItineraryRequestDto>> ConvertItineraryToGetFlightRequestDto(List<Itinerary> itineryList)
        {
            List<GetItineraryRequestDto> flights = new List<GetItineraryRequestDto>();
            foreach (var itinerary in itineryList)
            {
                flights.Add(new GetItineraryRequestDto()
                {
                    Flight_Id = itinerary.Flight_Id,
                    DepartureAt = itinerary.DepartureAt,
                    ArrivalAt = itinerary.ArrivalAt,
                    Currency = itinerary.Prices == null ? "" : itinerary.Prices.Currency,
                    Adult = itinerary.Prices == null? 0: itinerary.Prices.Adult,
                    Child = itinerary.Prices == null ? 0 : itinerary.Prices.Child
                });
            }
            return flights;

        }
    }
}
