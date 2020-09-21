using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTMS.Core.Extentions;
using PTMS.Core.Models;
using PTMS.Core.Repositories;

namespace PTMS.API.Controllers {
	[Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository) {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

		[HttpGet]
		public async Task<IResponse<IEnumerable<User>>> GetAsync() => new Response<IEnumerable<User>>((await _userRepository.Get()).Select(x=>x.WithoutPassword()));

		[HttpGet("{id}")]
		public async Task<ActionResult<IResponse<User>>> GetAsync(string id) {
			var user = await _userRepository.Get(id);

			if (user == null) {
				return NotFound();
			}

			return new Response<User>(user);
		}

		[HttpPut]
		public async Task<ActionResult<User>> CreateAsync(User user) {
			user.Creator = User.Identity.Name;
			var created = await _userRepository.Create(user);

			return Ok(created);
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> UpdateAsync(string id, User userIn) {
			userIn.Editor = User.Identity.Name;
			userIn.Updated = (long)DateTime.UtcNow
			   .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
			   .TotalMilliseconds;
			await _userRepository.Update(id, userIn);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(string id) {
			await _userRepository.Delete(id);

			return NoContent();
		}
	}
}
