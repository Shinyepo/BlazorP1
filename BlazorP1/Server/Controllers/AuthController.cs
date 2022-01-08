using BlazorP1.Server.Data;
using BlazorP1.Server.Services;
using BlazorP1.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegister request)
        {
            var response = await _authRepo.Register(
                new User
                {
                    Username = request.Username,
                    Email = request.Email
                }, request.Password, request.StartUnitId);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin request)
        {
            var response = await _authRepo.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("RequestPasswordChange")]
        public async Task<IActionResult> ChangePassword([FromBody]string email)
        {
            var response = await _authRepo.RequestPasswordChange(email);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordChangeForm request)
        {
            var response = await _authRepo.ChangePassword(request.Password, request.Secret);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
