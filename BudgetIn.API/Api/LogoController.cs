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
                IconUrl = logoViewModel.IconUrl
            };

            bool success = await _repository.AddAsync(logo);

            if (!success) return StatusCode(500);

            return StatusCode(201);
        }

        // GET: api/Logo/{id}
        // ROLE admin

        // PUT: api/Logo/{id}
        // ROLE admin

        // DELETE: api/Logo/{id}
        // ROLE admin
    }
}
