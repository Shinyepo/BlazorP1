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
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorP1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserUnitController : ControllerBase
    {
        private readonly IUtilityService _UtilityService;
        private readonly DataContext _Context;

        public UserUnitController(DataContext context, IUtilityService UtilityService)
        {
            _Context = context;
            _UtilityService = UtilityService;
        }


        [HttpPost]
        public async Task<IActionResult> BuildUserUnit([FromBody]int UnitId)
        {
            var unit = await _Context.Units.FirstOrDefaultAsync(x=>x.Id == UnitId);
            var user = await _UtilityService.GetUser();
            var unitcount = _Context.UserUnits.Where(x => x.UnitId == unit.Id && x.UserId == user.Id).Count();
            var UnitBuilding = await _Context.UnitBuildings.FirstOrDefaultAsync(x => x.UnitId == unit.Id);
            var UtilityBuilding = await _Context.UtilityBuildings.FirstOrDefaultAsync(x => x.BuildingName == StaticDetails.RecruitUtilityBuilding);
            var UserBuildings = await _Context.UserBuildings.Where(x => x.UserId == user.Id).ToListAsync();
            var UserArmyBuilding = UserBuildings.FirstOrDefault(x => x.BuildingType == StaticDetails.UnitBuilding && x.BuildingId == UnitBuilding.Id && x.BuildingLevel > 0);
            var UserUtilityBuilding = UserBuildings.FirstOrDefault(x => x.BuildingType == StaticDetails.UtilityBuilding && x.BuildingId == UtilityBuilding.Id && x.BuildingLevel > 0);
            var cost = unit.BananaCost;
            int limit = 0;

            if (UserArmyBuilding == null)
            {
                limit = UnitBuilding.UnitLimitBase;
            } 
            else
            {
                limit =  UnitBuilding.UnitLimitBase + (UnitBuilding.UnitLimitStep * UserArmyBuilding.BuildingLevel);
            }
            if (unitcount >= limit)
            {
                return BadRequest("You cant recruit more units. Upgrade unit's building if possible.");
            }
            if (UserUtilityBuilding != null)
            {
                var calc = (double)(UtilityBuilding.BonusAmountBase + (UtilityBuilding.BonusAmountStep * UserUtilityBuilding.BuildingLevel)) / 100;
                cost = Convert.ToInt32(Math.Round(cost * (1 - calc)));
            }

            if (cost > user.Bananas)
            {
                return BadRequest("Not Enough Bananas to buy Unit!");
            }

            user.Bananas -= cost;
            var UserUnit = new UserUnit
            {
                UnitId = unit.Id,
                HitPoints = unit.HitPoints,
                UserId = user.Id
            };
            _Context.UserUnits.Add(UserUnit);
            await _Context.SaveChangesAsync();
            return Ok(unit.Title);

        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnit()
        {
            var user = await _UtilityService.GetUser();
            await Task.Delay(200);
            var Units = await _Context.UserUnits.Include(x=>x.Unit).Where(x => x.UserId == user.Id).OrderBy(x=>x.Unit.Title).ToListAsync();


            return Ok(Units);
        }

        [HttpPost("revive")]
        public async Task<IActionResult> ReviveArmy()
        {
            var user = await _UtilityService.GetUser();
            var units =  await _Context.UserUnits.Where(x=>x.UserId == user.Id).Include(x=>x.Unit).ToListAsync();
            var UtilityBuilding = await _Context.UserBuildings.FirstOrDefaultAsync(x => x.BuildingType == StaticDetails.UtilityBuilding && x.UserId == user.Id && x.BuildingId == 1 && x.BuildingLevel > 0);
            int cost = 0;
            var healcost = StaticDetails.HealCost;
            if (UtilityBuilding != null)
            {
                var BuildingConfig = await _Context.UtilityBuildings.FirstOrDefaultAsync(x => x.Id == UtilityBuilding.BuildingId);                
                var calc = (double)(BuildingConfig.BonusAmountBase + (BuildingConfig.BonusAmountStep * UtilityBuilding.BuildingLevel)) / 100;
                healcost = Convert.ToInt32(Math.Round(healcost * (1 - calc)));

            }
            foreach (var item in units)
            {
                cost += Convert.ToInt32(Math.Round((decimal)((100 - item.HitPoints) * healcost) / 100));
            }            

            if (user.Bananas < cost)
            {
                return BadRequest($"Not enough bananas! You need {cost} bananas to heal the army");
            }

            bool armyalive = true;
            foreach (var item in units)
            {
                if (item.HitPoints <= 99)
                {
                    armyalive = false;
                    item.HitPoints = 100;
                }
            }

            if (armyalive)
            {
                return Ok("Your army is already healed!");
            }

            user.Bananas -= cost;

            await _Context.SaveChangesAsync();

            return Ok("Army healed!");

        }
    }
}
