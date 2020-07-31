using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BudgetIn.API.Models;
using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BudgetIn.API.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LogoController : ControllerBase
    {
        private readonly ILogoRepository _repository;

        public LogoController(ILogoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Logo/
        [HttpGet]
        // ROLE users
        public async Task<IActionResult> List()
        {
            var logos = await _repository.ListAsync();
            
            //TODO поменять на responseModel (во всех контроллерах)
            return Ok(logos);
        }

        // POST: api/Logo
        // ROLE admin
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] LogoViewModel logoViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Logo logo = new Logo()
            {
                Name = logoViewModel.Name,
                IconUrl = logoViewModel.IconUrl
            };

            bool success = await _repository.AddAsync(logo);

            if (!success) return StatusCode(500);

            return StatusCode(201);
        }

        // GET: api/Logo/{id}
        // ROLE admin

        // DELETE: api/Logo/{id}
        // ROLE admin
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            Logo logo = await _repository.GetByIdAsync(id);

            if (logo == null) return NotFound();

            if (await _repository.DeleteAsync(logo))
            {
                return Ok();
            } else
            {
                return StatusCode(500);
            }
        }

        // PUT: api/Logo/{id}
        // ROLE admin
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] LogoViewModel logoViewModel, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Logo logo = await _repository.GetByIdAsync(id);

            if (logo == null) return NotFound();

            logo.Name = logoViewModel.Name;
            logo.IconUrl = logoViewModel.IconUrl;

            if (await _repository.UpdateAsync(logo))
            {
                return Ok();
            } else
            {
                return StatusCode(500);
            }
        }


    }
}
