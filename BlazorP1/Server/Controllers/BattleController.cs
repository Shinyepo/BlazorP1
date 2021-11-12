using BlazorP1.Server.Data;
using BlazorP1.Server.Services;
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
    [Authorized]
    public class BattleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _UtilityService;


        public BattleController(DataContext context, IUtilityService UtilityService)
        {
            _context = context;
            _UtilityService = UtilityService;
        }

        [HttpPost("startbattle")]
        public async Task<IActionResult> StartBattle([FromBody] int OpponentId)
        {
            var attacker = await _UtilityService.GetUser();
            var Opponent = await _context.Users.FindAsync(OpponentId);
            if (Opponent == null || Opponent.isDeleted)
            {
                return NotFound("Opponent not found.");
            }
            var result = new BattleResult();
            await Fight(attacker, Opponent, result);
            return Ok(result);
        }

        [HttpPost("startmission")]
        public async Task<IActionResult> StartMission([FromBody] string MissionLevel)
        {
            var attacker = await _UtilityService.GetUser();
            var mission = await _context.MissionLevels.FirstOrDefaultAsync(x => x.MissionName == MissionLevel);
            if (mission == null)
            {
                return NotFound("Mission not found.");
            }
            var result = new BattleResult();
            var opponent = new User
            {
                Id = mission.Id,
                Username = mission.MissionName,
                Battles = mission.Battles,
                Wins = mission.Wins,
                Defeats = mission.Defeats
            };
            await FightMission(attacker, opponent, result);
            return Ok(result);
        }

        private async Task FightMission(User attacker, User mission, BattleResult result)
        {
            var attackerArmy = await _context.UserUnits.Where(x => x.UserId == attacker.Id && x.HitPoints > 0)
                .Include(x => x.Unit)
                .ToListAsync();

            var OpponentArmy = await _context.MissionUnits.Where(x => x.MissionLevelId == mission.Id && x.HitPoints > 0)
                    .Include(x => x.Unit)
                    .ToListAsync();
            var attackerDmgSum = 0;
            var opponentDmgSum = 0;
            int currRound = 0;

            while (attackerArmy.Count > 0 && OpponentArmy.Count > 0)
            {
                currRound++;

                if (currRound % 2 != 0)
                {
                    attackerDmgSum += FightMissionRound(attacker, mission, attackerArmy, OpponentArmy, result, "user");
                }
                else
                {
                    opponentDmgSum += FightMissionRound(attacker, mission, attackerArmy, OpponentArmy, result, "mission");
                }

            }
            result.IsVictory = OpponentArmy.Count == 0;
            result.RoundsFought = currRound;

            if (result.RoundsFought > 0)
            {
                var miss = await _context.MissionLevels.FindAsync(mission.Id);
                await FinishMissionFight(attacker, miss, result, attackerDmgSum, opponentDmgSum);
            }


        }

        private async Task Fight(User attacker, User opponent, BattleResult result)
        {
            var attackerArmy = await _context.UserUnits.Where(x => x.UserId == attacker.Id && x.HitPoints > 0)
                .Include(x => x.Unit)
                .ToListAsync();

            var OpponentArmy = await _context.UserUnits.Where(x => x.UserId == opponent.Id && x.HitPoints > 0)
                    .Include(x => x.Unit)
                    .ToListAsync();
            var attackerDmgSum = 0;
            var opponentDmgSum = 0;
            int currRound = 0;

            while (attackerArmy.Count > 0 && OpponentArmy.Count > 0)
            {
                currRound++;

                if (currRound % 2 != 0)
                {
                    attackerDmgSum += FightRound(attacker, opponent, attackerArmy, OpponentArmy, result, attacker);
                }
                else
                {
                    opponentDmgSum += FightRound(opponent, attacker, OpponentArmy, attackerArmy, result, attacker);
                }

            }
            result.IsVictory = OpponentArmy.Count == 0;
            result.RoundsFought = currRound;

            if (result.RoundsFought > 0)
            {
                await FinishFight(attacker, opponent, result, attackerDmgSum, opponentDmgSum);
            }


        }

        private async Task FinishFight(User attacker, User opponent, BattleResult result, int attackerDmgSum, int opponentDmgSum)
        {
            result.AttackerDmgSum = attackerDmgSum;
            result.OpponentDmgSum = opponentDmgSum;
            attacker.Battles += 1;
            opponent.Battles += 1;

            if (result.IsVictory)
            {
                attacker.Wins++;
                opponent.Defeats++;
                attacker.Bananas += (int)(attackerDmgSum * 1.3);
                opponent.Bananas += (int)(opponentDmgSum * 0.7);
                result.Reward = (int)(attackerDmgSum * 1.3);
            }
            else
            {
                opponent.Wins++;
                attacker.Defeats++;
                attacker.Bananas += (int)(attackerDmgSum * 0.7);
                opponent.Bananas += (int)(opponentDmgSum * 1.3);
                result.Reward = (int)(attackerDmgSum * 0.7);
            }

            StoreBattleHistory(attacker, opponent, result);

            await _context.SaveChangesAsync();
        }

        private async Task FinishMissionFight(User attacker, MissionLevel opponent, BattleResult result, int attackerDmgSum, int opponentDmgSum)
        {
            result.AttackerDmgSum = attackerDmgSum;
            result.OpponentDmgSum = opponentDmgSum;
            opponent.Battles += 1;

            if (result.IsVictory)
            {
                opponent.Defeats++;
                attacker.Bananas += (int)(attackerDmgSum * 1.3);
                result.Reward = (int)(attackerDmgSum * 1.3);

            }
            else
            {
                opponent.Wins++;
            }
            _context.MissionUnits.Where(x => x.MissionLevelId == opponent.Id).ToList().ForEach(x => x.HitPoints = 100);
            await _context.SaveChangesAsync();
        }

        private void StoreBattleHistory(User attacker, User opponent, BattleResult result)
        {
            var battle = new Battle();
            battle.Attacker = attacker;
            battle.Opponent = opponent;
            battle.RoundsFought = result.RoundsFought;
            battle.WinnerDamage = result.IsVictory ? result.AttackerDmgSum : result.OpponentDmgSum;
            battle.Winner = result.IsVictory ? attacker : opponent;

            _context.Battles.Add(battle);
        }

        private int FightRound(User attacker, User opponent, List<UserUnit> attackerArmy, List<UserUnit> opponentArmy, BattleResult result, User Agressor)
        {
            int randomAI = new Random().Next(attackerArmy.Count);
            int randomOI = new Random().Next(opponentArmy.Count);

            var randomAttacker = attackerArmy[randomAI];
            var randomOpponent = opponentArmy[randomOI];

            var damage = new Random().Next(randomAttacker.Unit.Attack) - new Random().Next(randomOpponent.Unit.Defence);
            if (damage < 0) damage = 0;
            var imgsrc = attacker.Username == Agressor.Username ? "swords" : "shield";
            if (damage <= randomOpponent.HitPoints)
            {
                randomOpponent.HitPoints -= damage;
                result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>{attacker.Username}</b>'s {randomAttacker.Unit.Title} attacks " +
                    $"<b>{opponent.Username}</b>'s {randomOpponent.Unit.Title} with {damage} damage");
                return damage;
            }
            else
            {
                damage = randomOpponent.HitPoints;
                randomOpponent.HitPoints = 0;
                opponentArmy.Remove(randomOpponent);
                result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>{attacker.Username}</b>'s {randomAttacker.Unit.Title} kills " +
                    $"<b>{opponent.Username}</b>'s {randomOpponent.Unit.Title}!");
                if (Agressor.Username == attacker.Username)
                {
                    result.OpponentKilledUnits += 1;
                }
                else
                {
                    result.AttackerKilledUnits += 1;
                }
                return damage;
            }


        }

        private int FightMissionRound(User attacker, User mission, List<UserUnit> attackerArmy, List<MissionUnits> missionArmy, BattleResult result, string Agressor)
        {
            if (Agressor == "user")
            {

                int randomAI = new Random().Next(attackerArmy.Count);
                int randomOI = new Random().Next(missionArmy.Count);

                var randomAttacker = attackerArmy[randomAI];
                var randomOpponent = missionArmy[randomOI];

                var damage = new Random().Next(randomAttacker.Unit.Attack) - new Random().Next(randomOpponent.Unit.Defence);
                if (damage < 0) damage = 0;
                var imgsrc = "swords";
                if (damage <= randomOpponent.HitPoints)
                {
                    randomOpponent.HitPoints -= damage;
                    result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>{attacker.Username}</b>'s {randomAttacker.Unit.Title} attacks " +
                        $"<b>Mission({mission.Username})</b>'s {randomOpponent.Unit.Title} with {damage} damage");
                    return damage;
                }
                else
                {
                    damage = randomOpponent.HitPoints;
                    randomOpponent.HitPoints = 0;
                    missionArmy.Remove(randomOpponent);
                    result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>{attacker.Username}</b>'s {randomAttacker.Unit.Title} kills " +
                        $"<b>Mission({mission.Username})</b>'s {randomOpponent.Unit.Title}!");

                    result.OpponentKilledUnits += 1;

                    return damage;
                }

            }
            else
            {
                int randomAI = new Random().Next(missionArmy.Count);
                int randomOI = new Random().Next(attackerArmy.Count);

                var randomAttacker = missionArmy[randomAI];
                var randomOpponent = attackerArmy[randomOI];

                var damage = new Random().Next(randomAttacker.Unit.Attack) - new Random().Next(randomOpponent.Unit.Defence);
                if (damage < 0) damage = 0;
                var imgsrc = "shield";
                if (damage <= randomOpponent.HitPoints)
                {
                    randomOpponent.HitPoints -= damage;
                    result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>Mission({mission.Username})</b>'s {randomAttacker.Unit.Title} attacks " +
                        $"<b>{attacker.Username}</b>'s {randomOpponent.Unit.Title} with {damage} damage");
                    return damage;
                }
                else
                {
                    damage = randomOpponent.HitPoints;
                    randomOpponent.HitPoints = 0;
                    attackerArmy.Remove(randomOpponent);
                    result.Log.Add($"<img src=\"icons/{imgsrc}.png\" height=\"24px\" width=\"24px\" class=\"img-fluid\"/> <b>Mission({mission.Username})</b>'s {randomAttacker.Unit.Title} kills " +
                        $"<b>{attacker.Username})</b>'s {randomOpponent.Unit.Title}!");

                    result.AttackerKilledUnits += 1;

                    return damage;
                }
            }

        }
    }
}
