using FlightFinderApi.Models;
using FlightFinderApi.ViewModels;

namespace FlightFinderApi.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUser(AddUserRequestDto addUserRequestDto);
        Task<List<GetUserRequestDto>> GetAllUsersData();
        Task DeleteUserById(int id);
        Task<User> VerifyUser(string username, string password);
    }
}
