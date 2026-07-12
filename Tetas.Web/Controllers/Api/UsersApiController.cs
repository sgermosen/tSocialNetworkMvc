namespace Tetas.Web.Controllers.Api
{
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Tetas.Web.Models.Api;

    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersApiController : ControllerBase
    {
        private readonly IModeration _moderation;
        private readonly IUserHelper _userHelper;

        public UsersApiController(IModeration moderation, IUserHelper userHelper)
        {
            _moderation = moderation;
            _userHelper = userHelper;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet("blocked")]
        public async Task<ActionResult<IEnumerable<BlockedUserDto>>> Blocked()
        {
            var blocked = await _moderation.GetBlockedUsersAsync(UserId);
            return Ok(blocked.Select(u => new BlockedUserDto
            {
                Email = u.Email,
                FullName = u.FullName
            }));
        }

        [HttpPost("{email}/block")]
        public async Task<IActionResult> Block(string email)
        {
            var target = await _userHelper.GetUserByEmailAsync(email);
            if (target == null)
            {
                return NotFound();
            }

            var ok = await _moderation.BlockAsync(UserId, target.Id);
            return ok ? NoContent() : BadRequest();
        }

        [HttpDelete("{email}/block")]
        public async Task<IActionResult> Unblock(string email)
        {
            var target = await _userHelper.GetUserByEmailAsync(email);
            if (target == null)
            {
                return NotFound();
            }

            await _moderation.UnblockAsync(UserId, target.Id);
            return NoContent();
        }
    }
}
