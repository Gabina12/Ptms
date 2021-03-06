﻿using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PTMS.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PTMS.API.Controllers {
	//[Authorize]
    [Route("api/templates")]
	public class RenderController : Controller
	{
		private readonly IRenderingService _render;
		private readonly ILogger<RenderController> _logger;

        public RenderController(IRenderingService render, ILogger<RenderController> logger) {
            _render = render ?? throw new ArgumentNullException(nameof(render));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("{id}/render/txt/{version}")]
		public async Task<IActionResult> Render(string id, string version, [FromBody]JObject templateIn)
		{
			_logger.LogInformation($"start rendering for id: {id}, version: {version} - {DateTime.Now}");
			object json = null;
			if (templateIn != null)
				json = JsonConvert.DeserializeObject(templateIn.ToString());
			return Ok(await _render.RenderAsync(id, "txt", version, json));
		}


		[HttpPost("{id}/render/pdf/{version}")]
		public async Task<IActionResult> RenderPdf(string id, string version, [FromBody] JObject templateIn) {
			object json = null;
			if (templateIn != null)
				json = JsonConvert.DeserializeObject(templateIn.ToString());
			var file = await _render.RenderPdfAsync(id, "pdf", version, json);
			return File(file, "application/pdf", $"{id}.pdf");
		}

	}
}
