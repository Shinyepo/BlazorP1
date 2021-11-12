using BlazorP1.Server.Data;
using BlazorP1.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DataContext _context;

        public UnitController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var Units = await _context.Units.ToListAsync();
            return Ok(Units);
        }


    }
}
