using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using BudgetIn.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetIn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Category/
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await _repository.ListAsync();

            return Ok(categories);
        }

        // POST: api/Category
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Category category = new Category()
            {
                Name = categoryViewModel.Name,
                LogoId = categoryViewModel.LogoId,
                //TODO изменить userId
                UserId = "userid123123"
            };

            if (await _repository.AddAsync(category))
            {
                return StatusCode(201);
            } else
            {
                return StatusCode(500);
            }

        }

        // GET: api/Category/{id}
        [HttpGet("{id}:int")]
        public async Task<IActionResult> GetById(int id)
        {
            Category category = await _repository.GetByIdAsync(id);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // PUT: api/Category/{id}
        [HttpPost("{id}:int")]
        public async Task<IActionResult> Update([FromBody] CategoryViewModel categoryViewModel, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _repository.GetByIdAsync(id) == null) return NotFound();

            Category category = new Category()
            {
                Name = categoryViewModel.Name,
                LogoId = categoryViewModel.LogoId,
                //TODO изменить userId
                UserId = "userid123123"
            };

            if (await _repository.UpdateAsync(category))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/Category/{id}
    }
}
