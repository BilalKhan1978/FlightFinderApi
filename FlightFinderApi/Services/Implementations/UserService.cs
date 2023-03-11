using AutoMapper;
using FlightFinderApi.Data;
using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
            var emailValidation = await _dbContext.Users.Where(x => x.UserName == addUserRequestDto.UserName).FirstOrDefaultAsync();
            if (emailValidation != null) throw new Exception("This username has already been taken. kindly try another one");
            CreatePasswordHash(addUserRequestDto.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);
            var user = new User
            {
                UserName = addUserRequestDto.UserName,
                PassHash = passwordHash,
                PassSalt = passwordSalt
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<List<GetUserRequestDto>> GetAllUsersData()
        //{
        //    var users = await _dbContext.Users.ToListAsync();
        //    return _mapper.Map<List<GetUserRequestDto>>(users);
        //}

        //public async Task DeleteUserById(int id)
        //{
        //  var user = await _dbContext.Users.FindAsync(id);
        //  if (user == null) throw new Exception("No such user exists");
        //  _dbContext.Users.Remove(user);
        //  await _dbContext.SaveChangesAsync();
        //}

        public async Task<User> VerifyUser(string username, string password)
        {
            var user = await _dbContext.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("Username / Password is incorrect");
            // if user is not null
            if (!VerifyPinHash(password, user.PassHash, user.PassSalt))
                throw new Exception("You have entered incorrect Password");

            // Authentication verified
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new Exception("Pin");
            // verify pin
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }


    }
}
