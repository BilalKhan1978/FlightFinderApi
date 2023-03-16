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
            var date = DateTime.Parse(searchRootRequestDto.DepartureTime);
            var roots = await _dbContext.Roots.Include(x => x.Itineraries.Where(y=>y.DepartureAt.Date == date.Date)).ThenInclude(x => x.Prices)
                            .FirstOrDefaultAsync(x => x.DepartureDestination == searchRootRequestDto.DepartureDestination &&
                                        x.ArrivalDestination == searchRootRequestDto.ArrivalDestination);
            if (roots == null) throw new Exception("No flight found");
            return await ConvertRootsToGetRootsRequestDto(roots);
        }

        public async Task<List<GetConnectedFlightRequestDto>> SearchConnectedRoots(SearchRootRequestDto searchRootRequestDto)
        {
            var date = DateTime.Parse(searchRootRequestDto.DepartureTime);
            List<Root> roots= new List<Root>();
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
            if (flights == null || flights.Count == 0) throw new Exception("No flight found for this rout");
                foreach (var flight in flights)
                {
                    roots = new List<Root>() { new Root()
                {
                    DepartureDestination=flight.Origin1,
                    ArrivalDestination=flight.Destination1,
                    Itineraries=flight.Flight1.Where(x=>x.DepartureAt.Date == date.Date).ToList()

                },
                new Root()
                {
                    DepartureDestination=flight.Origin2,
                    ArrivalDestination=flight.Destination2,
                    Itineraries=flight.Flight2.Where(x=>x.DepartureAt.Date == date.Date
                    ).ToList()

                }
                    };
                }
                Dictionary<string, List<Itinerary>> connectedFlightsDictionary = new Dictionary<string, List<Itinerary>>();
                foreach(var itenary in roots[0].Itineraries)
                {
                    connectedFlightsDictionary.Add(itenary.Flight_Id, roots[1].Itineraries.Where(x=>x.DepartureAt > itenary.ArrivalAt).OrderBy(flight => flight.DepartureAt).ToList());

                }
            var connectedFlightList = await ConvertRootsToGetConnectedFlightRequestDto(roots, connectedFlightsDictionary);
            return connectedFlightList;
        }
       
        private async Task<List<GetConnectedFlightRequestDto>> ConvertRootsToGetConnectedFlightRequestDto(List<Root> rootList, Dictionary<string, List<Itinerary>> connectedFlightsDictionary)
        {
            List<GetConnectedFlightRequestDto> getConnectedFlightRequestDto = new List<GetConnectedFlightRequestDto>();
            foreach(var item in connectedFlightsDictionary)
            {
                var flight = rootList[0].Itineraries.FirstOrDefault(x => x.Flight_Id == item.Key);
                if (item.Value == null || item.Value.Count==0) continue;
                foreach(var connectedflight in item.Value)
                {
                    
                    getConnectedFlightRequestDto.Add(new GetConnectedFlightRequestDto()
                    {
                        FirstRoot = new GetRootsRequestDto()
                        {
                            DepartureDestination = rootList[0].DepartureDestination,
                            ArrivalDestination = rootList[0].ArrivalDestination,
                            FlightRequests = await ConvertItineraryToGetFlightRequestDto(flight)
                        },
                        SecondRoot = new GetRootsRequestDto()
                        {
                            DepartureDestination = rootList[1].DepartureDestination,
                            ArrivalDestination = rootList[1].ArrivalDestination,
                            FlightRequests = await ConvertItineraryToGetFlightRequestDto(connectedflight)
                        },
                    });
                }
                
            }
            return getConnectedFlightRequestDto;
        }
        private async Task<List<GetRootsRequestDto>> ConvertRootsToGetRootsRequestDto(Root root)
        {
            List<GetRootsRequestDto> getRootsRequestDto = new List<GetRootsRequestDto>();
            foreach (var flight in root.Itineraries)
            {
                getRootsRequestDto.Add(new GetRootsRequestDto()
                {
                    DepartureDestination = root.DepartureDestination,
                    ArrivalDestination = root.ArrivalDestination,   
                    FlightRequests = await ConvertItineraryToGetFlightRequestDto(flight)

                });
            }
            return getRootsRequestDto;
        }

        private async Task<GetItineraryRequestDto> ConvertItineraryToGetFlightRequestDto(Itinerary itinerary)
        {
            List<GetItineraryRequestDto> flights = new List<GetItineraryRequestDto>();

            var price = await _dbContext.Itineraries.Include(x => x.Prices).FirstOrDefaultAsync(x => x.Flight_Id == itinerary.Flight_Id);
            return new GetItineraryRequestDto()
                {
                    Flight_Id = itinerary.Flight_Id,
                    DepartureAt = itinerary.DepartureAt.ToString("HH:mm"),
                    ArrivalAt = itinerary.ArrivalAt.ToString("HH:mm"),
                    AvailabeSeats=itinerary.AvailableSeats,
                    Currency = price.Prices.Currency,
                    Adult = price.Prices.Adult,
                    Child = price.Prices.Child
                };
            
            //return flights;

        }
    }
}
