using BlazorP1.Server.Data;
using BlazorP1.Server.Services;
using BlazorP1.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorP1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IUtilityService _UtilityService;

        public UserController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _UtilityService = utilityService;
        }


        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {
            var user = await _UtilityService.GetUser();

            return Ok(user.Bananas);
        }

        [HttpPut("addbananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await _UtilityService.GetUser();
            user.Bananas += bananas;

            await _context.SaveChangesAsync();

            return Ok(user.Bananas);
        }

        [HttpGet("sincelastlogin")]
        public async Task<IActionResult> SinceLastLogin()
        {
            var user = await _UtilityService.GetUser();
            var battles = _context.Battles.Where(x => x.OpponentId == user.Id && x.BattleDate.CompareTo(user.LastLogin) > 0).ToList();
            return Ok(battles.Count());
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _UtilityService.GetUser();

            var battles = await _context.Battles.Where(x => x.AttackerId == user.Id || x.OpponentId == user.Id)
                .Include(x => x.Attacker)
                .Include(x => x.Opponent)
                .Include(x => x.Winner)
                .ToListAsync();

            var history = battles.Select(x => new BattleHistoryEntry
            {
                AttackerId = x.AttackerId,
                OpponentId = x.OpponentId,
                BattleId = x.Id,
                YouWon = x.WinnerId == user.Id,
                AttackerName = x.Attacker.Username,
                OpponentName = x.Opponent.Username,
                RoundsFought = x.RoundsFought,
                WinnerDamageDealt = x.WinnerDamage,
                BattleDate = x.BattleDate
            });
            return Ok(history.OrderByDescending(x => x.BattleDate));
        }

        [HttpPut("eatbananas")]
        public async Task<IActionResult> EatBananas([FromBody] int bananas)
        {
            var user = await _UtilityService.GetUser();
            user.Bananas -= bananas;

            await _context.SaveChangesAsync();

            return Ok(user.Bananas);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var list = await CreateLeaderboardListAsync();

            return Ok(list);
        }

        public async Task<IEnumerable<UserStatistic>> CreateLeaderboardListAsync()
        {
            var user = await _context.Users.Where(u => u.isDeleted != true && u.isConfirmed).ToListAsync();

            user = user.OrderByDescending(x => x.Wins)
                .ThenBy(x => x.Defeats)
                .ThenBy(x => x.RegistrationDate)
                .ToList();

            int rank = 1;
            var response = user.Select(u => new UserStatistic
            {
                Rank = rank++,
                UserId = u.Id,
                Battles = u.Battles,
                Defeats = u.Defeats,
                Username = u.Username,
                Wins = u.Wins
            });

            return response;
        }

        [HttpGet("myrank")]
        public async Task<IActionResult> GetMyRank()
        {
            var user = await _UtilityService.GetUser();
            var leaderboard = await CreateLeaderboardListAsync();
            var rank = leaderboard.FirstOrDefault(x => x.UserId == user.Id);
            if (rank == null)
            {
                return Ok(0);
            }
            return Ok(rank.Rank);

        }

        [HttpPut("llupdate")]
        public async Task<IActionResult> LastLoginUpdate([FromBody]bool value)
        {
            var user = await _UtilityService.GetUser();
            var fromdb = await _context.Users.FindAsync(user.Id);
            fromdb.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("Ok");
        }
    }
}
