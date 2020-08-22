using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BudgetIn.Front.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BudgetIn.Front.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var client = _clientFactory.CreateClient("budgetinapi");

            using var response = await client.PostAsync("User/Login", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return Ok(content);

            return BadRequest(content); // TODO придумать что нибудь с этим
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var client = _clientFactory.CreateClient("budgetinapi");

            using var response = await client.PostAsync("User/Register", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return Ok(content);

            return BadRequest(content); // TODO придумать что нибудь с этим
        }
    }
}
