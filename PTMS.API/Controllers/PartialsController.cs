using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTMS.Core.Models;
using PTMS.Core.Services.Interfaces;

namespace PTMS.API.Controllers {

	[Authorize]
	[Route("api/partials")]
	[ApiController]

	public class PartialsController : ControllerBase {
		// GET: /<controller>/
		private readonly IPartialsService _templateService;

		public PartialsController(IPartialsService templateService) {
			_templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
		}

		[HttpGet]
		public async Task<IResponse<IEnumerable<PartialTemplate>>> GetAsync() => await _templateService.Get();

		[HttpGet("{id}")]
		public async Task<ActionResult<IResponse<PartialTemplate>>> GetAsync(string id) {
			var template = await _templateService.Get(id);

			if (template == null) {
				return NotFound();
			}

			return new Response<PartialTemplate>(template);
		}

		[HttpPut]
		public async Task<ActionResult<PartialTemplate>> CreateAsync(PartialTemplate template) {
			template.Creator = User.Identity.Name;
			var created = await _templateService.Create(template);

			return Ok(created);
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> UpdateAsync(string id, PartialTemplate templateIn) {
			templateIn.Editor = User.Identity.Name;
			
			await _templateService.Update(id, templateIn);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(string id) {
			await _templateService.Delete(id);

			return NoContent();
		}
	}
}
