using FlightFinderApi.Models;
using FlightFinderApi.ViewModels;



namespace FlightFinderApi.Services.Interfaces
{
    public interface IRootService
    {
      //Task AddRoots(List<Root> addRootsRequestDto);
      Task<List<GetRootsRequestDto>> SearchRoots(SearchRootRequestDto searchRootRequestDto);
        Task<List<GetConnectedFlightRequestDto>> SearchConnectedRoots(SearchRootRequestDto searchRootRequestDto);

    }
}
