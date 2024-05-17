using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseService firebaseService;
        private readonly UserManager<AppUser> _userManager;

        public FirebaseController(IFirebaseService firebaseService, UserManager<AppUser> userManager)
        {
            this.firebaseService = firebaseService;
            this._userManager = userManager;
        }

        [Authorize]
        [HttpPost, Route("save-token")]
        public async Task<ActionResult<FirebaseToken>> CreateFirebaseToken(FirebaseToken payload)
        {
            try
            {

                // Retrieve userId from token
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                FirebaseToken tokens = await firebaseService.CreateNewToken(payload, userId);
                return Ok(tokens);
            }
            catch (InvalidProgramException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
