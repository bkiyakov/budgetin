using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var client = _clientFactory.CreateClient("budgetinapi");

            using var response = await client.PostAsync("User/Login", new StringContent(JsonSerializer.Serialize(model)));

            if (response.IsSuccessStatusCode) return Ok(response.Content);

            return BadRequest(); // TODO придумать что нибудь с этим
        }
    }
}
