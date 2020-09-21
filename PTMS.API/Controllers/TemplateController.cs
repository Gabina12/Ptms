using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PTMS.Core.Models;
using PTMS.Core.Services.Interfaces;
using PTMS.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PTMS.API.Controllers
{
	[Authorize]
	[Route("api/templates")]
	[ApiController]

	public class TemplateController : ControllerBase
	{
		// GET: /<controller>/
		private readonly ITemplateService _templateService;

		public TemplateController(ITemplateService templateService)
		{
			_templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
		}

		[HttpGet]
		public async Task<IResponse<IEnumerable<Template>>> GetAsync() => await _templateService.Get();

		[HttpGet("{id}")]
		public async Task<ActionResult<IResponse<Template>>> GetAsync(string id)
		{
			var template = await _templateService.Get(id);

			if (template == null) {
				return NotFound();
			}

			return new Response<Template>(template);
		}

		[HttpPut]
		public async Task<ActionResult<Template>> CreateAsync(Template template) {
			template.Creator = User.Identity.Name;
			var created = await _templateService.Create(template);

			return Ok(created);
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> UpdateAsync(string id, Template templateIn)
		{
			templateIn.Editor = User.Identity.Name;
			await _templateService.Update(id, templateIn);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(string id)
		{
			await _templateService.Delete(id);

			return NoContent();
		}
	}
}
