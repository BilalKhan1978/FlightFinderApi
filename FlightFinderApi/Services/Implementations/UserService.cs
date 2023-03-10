using AutoMapper;
using FlightFinderApi.Data;
using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightFinderApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly FlightFinderDbContext _dbContext;
        IMapper _mapper;
        public UserService(FlightFinderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddUser(AddUserRequestDto addUserRequestDto)
        {
            var user = _mapper.Map<User>(addUserRequestDto);
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GetUserRequestDto>> GetAllUsersData()
        {
            var users = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<GetUserRequestDto>>(users);
        }
        public async Task DeleteUserById(int id)
        {
          var user = await _dbContext.Users.FindAsync(id);
          if (user == null) throw new Exception("No such user exists");
          _dbContext.Users.Remove(user);
          await _dbContext.SaveChangesAsync();
        }

        public async Task<User> VerifyUser(string username, string password)
        {
            var user = await _dbContext.Users.Where(x => x.UserName == username && x.Password == password).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("Username / Password is incorrect"); 

            // Authentication verified
            return user;
        }


    }
}
