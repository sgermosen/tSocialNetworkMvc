namespace Tetas.Web.Controllers.Api
{
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Web.Models.Api;

    [ApiController]
    [Route("api/groups")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupsApiController : ControllerBase
    {
        private readonly IGroup _groupRepository;
        private readonly IUserHelper _userHelper;

        public GroupsApiController(IGroup groupRepository, IUserHelper userHelper)
        {
            _groupRepository = groupRepository;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> Get()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            var groups = await _groupRepository.GetPublicAndMyGroupsAsync(user.Id);

            return Ok(groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                TypeName = g.Type?.Name,
                PrivacyName = g.Privacy?.Name,
                IsAdmin = g.IsAdmin,
                IsMember = g.State
            }));
        }
    }
}
