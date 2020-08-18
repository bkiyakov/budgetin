using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using BudgetIn.WebApi.Identity.Models;
using BudgetIn.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetIn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly UserManager<User> _userManager;

        public CategoryController(ICategoryRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // GET: api/Category/
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            var categories = await _repository.ListAsync(userId);

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

            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            Category category = new Category()
            {
                Name = categoryViewModel.Name,
                LogoId = categoryViewModel.LogoId,
                UserId = userId
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
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            Category category = await _repository.GetByIdAsync(id, userId);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}:int")]
        public async Task<IActionResult> Update([FromBody] CategoryViewModel categoryViewModel, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            if (await _repository.GetByIdAsync(id, userId) == null) return NotFound();

            Category category = new Category()
            {
                Id = id,
                Name = categoryViewModel.Name,
                LogoId = categoryViewModel.LogoId,
                UserId = userId
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
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            var category = await _repository.GetByIdAsync(id, userId);

            if (category == null) return NotFound();

            if (await _repository.DeleteAsync(category))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
