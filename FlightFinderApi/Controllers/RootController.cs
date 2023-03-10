using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RootController : ControllerBase
    {
        // Inject the service
        private readonly IRootService _rootService;
        public RootController(IRootService rootService)
        {
            _rootService = rootService;
        }

       // [HttpPost("allroots")]

        //public async Task<IActionResult> AddRoots(List<Root> addRootsRequestDto)
        //{
        //  try
        //  {
        //     await _rootService.AddRoots(addRootsRequestDto);
        //     return Ok("All records have been added"); 
        //  }
        //  catch(Exception e)
        //  {
        //    throw new Exception(e.Message); 
        //  }
        //}

        [HttpPost("search")]
        public async Task<IActionResult> SearchRoots([FromBody] SearchRootRequestDto searchRootRequestDto)
        {
            try
            {
                var roots = await _rootService.SearchRoots(searchRootRequestDto);
                if (roots!=null && roots.Count > 0) return Ok(roots);
                return NotFound("We are not offering any flight for this root");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
