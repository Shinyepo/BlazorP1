using BlazorP1.Server.Services;
using BlazorP1.Shared;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorP1.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        private IConfiguration _Config { get; }
        private readonly IUtilityService _UtilityService;
        public IEmailSender _emailSender { get; set; }

        public AuthRepository(DataContext context, IConfiguration config, IUtilityService utilityService, IEmailSender em)
        {
            _context = context;
            _Config = config;
            _UtilityService = utilityService;
            _emailSender = em;
        }
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()) && !x.isDeleted);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Authentication error!";
            } else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Authentication error!";
            } else
            {
                
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> RequestPasswordChange(string email)
        {
            var response = new ServiceResponse<string>();
            response.Message = "This feature is currently disabled";
            response.Success = false;

            return response;

            /*response.Message = "We sent a password reset request link to your email if the account exists.";

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                return response;
            }

            user.Secret = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();
            var content = "Here is your password reset request link : https://monkeyfights.shinyepo.dev/forgotpassword?Secret=" + user.Secret+"\n\n Ignore this message if you didnt request a password reset.";
            _emailSender.SendEmailAsync(email, "Password reset request - Do not reply - Monkey fights", content);
            response.Success = true;

            return response;*/

        }

        public async Task<ServiceResponse<string>> ChangePassword(string password, string secret)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Secret == secret);
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Secret = null;
            await _context.SaveChangesAsync();

            return new ServiceResponse<string> { Success = true, Message = "Successfuly changed password" };
        }

        public async Task<ServiceResponse<int>> Register(User user, string password, int startUnitId)
        {
            if (await EmailTaken(user.Email)) return new ServiceResponse<int> { Success = false, Message = "User already Exists."};
            if (await UsernameTaken(user.Username)) return new ServiceResponse<int> { Success = false, Message = "Username is already taken." };
            CreatePasswordHash(password,out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.isConfirmed = true;
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            await AddStartinUnit(user, startUnitId);

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
        }

        private async Task AddStartinUnit(User user, int startUnitId)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(x => x.Id == startUnitId);
            _context.UserUnits.Add(new UserUnit
            {
                UnitId = unit.Id,
                HitPoints = unit.HitPoints,
                UserId = user.Id,
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailTaken(string email)
        {
            if (await _context.Users.AnyAsync(x=>x.Email.ToLower().Equals(email.ToLower()))) return true;
            return false;
        }
        public async Task<bool> UsernameTaken(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()))) return true;            
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
