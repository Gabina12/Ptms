using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPCM.Core.Models;
using TPCM.Core.Repositories;

namespace TPCM.API.Controllers {
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase {

        private readonly ICategoryRepository _category;

        public CategoryController(ICategoryRepository category) {
            _category = category ?? throw new ArgumentNullException(nameof(category));
        }

        [Route("templates")]
        [HttpGet]
        public async Task<IResponse<IEnumerable<string>>> GetAsync() {
            return new Response<IEnumerable<string>>((await _category.Get()).Select(x => x.Name).ToArray());
        }

        [Route("partials")]
        [HttpGet]
        public async Task<IResponse<IEnumerable<string>>> GetPartialsAsync() {
            return new Response<IEnumerable<string>>((await _category.Get()).Select(x => x.Name).ToArray());
        }
    }
}
