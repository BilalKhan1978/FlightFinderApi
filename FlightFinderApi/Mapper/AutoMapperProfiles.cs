using AutoMapper;
using FlightFinderApi.Models;
using FlightFinderApi.ViewModels;

namespace FlightFinderApi.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Itinerary, GetItineraryRequestDto>().ForMember(
        dest => dest.Child,
        opt => opt.MapFrom(src => src.Prices.Child)).ForMember(
        dest => dest.Adult,
        opt => opt.MapFrom(src => src.Prices.Adult)).ForMember(
        dest => dest.Currency,
        opt => opt.MapFrom(src => src.Prices.Currency));

            CreateMap<AddUserRequestDto, User>();
            CreateMap<User, GetUserRequestDto>();

            
            
        }
    }
}
