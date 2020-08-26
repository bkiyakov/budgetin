using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BudgetIn.WebApi.Identity;
using BudgetIn.WebApi.Identity.Models;
using BudgetIn.WebApi.Identity.ViewModels;
using BudgetIn.WebApi.Identity.ViewModels.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BudgetIn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthday = model.Birthday
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if(createResult.Succeeded)
            {
                var createdUser = await _userManager.FindByNameAsync(user.UserName);

                var addRoleResult = await _userManager.AddToRoleAsync(createdUser, "User");

                if (addRoleResult.Succeeded)
                {
                    return StatusCode(201); // return Created()
                }
                else
                {
                    foreach (var error in addRoleResult.Errors)
                    {
                        ModelState.AddModelError("Error", error.Description);
                    }

                    return StatusCode(500, ModelState);
                }
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }

                return StatusCode(500, ModelState);
            }
        }

        [HttpPost]
        [Route("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                string token = await generateJwtToken(user);

                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                LoginResponseModel response = new LoginResponseModel()
                {
                    User = new LoginResponseModel.UserObject()
                    {
                        Username = user.UserName,
                        Role = userRoles.FirstOrDefault()
                    },
                    Token = token
                };

                return Ok(response);
            } else
            {
                ModelState.AddModelError("Username", "Username or password is invalid");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("List")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        // TODO Move to service class
        private async Task<string> generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = AuthOptions.GetSymmetricSecurityKey();
            var userRolesArray = (await _userManager.GetRolesAsync(user)).ToArray();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, userRolesArray.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
