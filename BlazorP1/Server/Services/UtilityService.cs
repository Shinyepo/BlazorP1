using BlazorP1.Server.Data;
using BlazorP1.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorP1.Server.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _http;


        public UtilityService(DataContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }


        public async Task<User> GetUser()
        {
            var userId = int.Parse(_http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }
    }
}
