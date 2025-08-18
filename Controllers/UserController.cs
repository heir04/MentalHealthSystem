using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService, JwtHelper jwtHelper) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userService.Login(loginDto);
            if (response.Status == false || response.Data == null)
            {
                return BadRequest(response);
            }

            if (response.Data != null)
            {
                var token = _jwtHelper.GenerateToken(response.Data.Email, response.Data.Role, response.Data.Id);
                return Ok(new
                {
                    Token = token
                });
            }
            return Unauthorized(response.Message);
        }

        [HttpPut("UpdatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(ChangePasswordDto userDto)
        {
            var response = await _userService.ChangePassword(userDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            var response = await _userService.Create(userDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateUserDto userDto)
        {
            var response = await _userService.Update(id, userDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAll();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetById()
        {
            var response = await _userService.Profile();
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}