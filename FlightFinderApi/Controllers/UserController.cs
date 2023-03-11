using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FlightFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDto addUserRequestDto)
        {
            try
            {
                await _userService.AddUser(addUserRequestDto);
                return Ok("User has been added");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        //[HttpGet("allusers")]
        //public async Task<IActionResult> GetAllUsersData()
        //{
        //  try
        //  {
        //    return Ok(await _userService.GetAllUsersData());
        //  }
        //  catch(Exception e)
        //  {
        //    throw new Exception(e.Message);
        //  }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserById(int id)
        //{
        //  try
        //  {
        //    await _userService.DeleteUserById(id);
        //    return Ok("Desired user has been deleted");
        //  }
        //  catch (Exception e)
        //  {
        //        if (e.Message.Contains("No such user exists"))
        //            return NotFound("No user found to delete");     
        //        throw new Exception(e.Message);
        //  }
        //}
    }
}

