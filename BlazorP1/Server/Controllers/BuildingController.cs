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
using System.Threading.Tasks;

namespace BlazorP1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BuildingController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public BuildingController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBuildings()
        {
            var user = await _utilityService.GetUser();
            var response = new BuildingViewModel
            {
                UserBuildings = await _context.UserBuildings.Where(x => x.UserId == user.Id).ToListAsync(),
                UnitBuildings = await _context.UnitBuildings.ToListAsync(),
                ProductionBuildings = await _context.ProductionBuildings.ToListAsync(),
                UtilityBuildings = await _context.UtilityBuildings.ToListAsync()
            };
            return Ok(response);
        }

        [HttpGet("getreadybuildings")]
        public async Task<IActionResult> GetReady()
        {
            var user = await _utilityService.GetUser();
            var response = _context.UserBuildings.Where(x => !x.isFinished && x.BuildFinishTime < DateTime.UtcNow && x.UserId == user.Id).Count();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> FinishBuilding(BuildingRequest request)
        {
            var User = await _utilityService.GetUser();
            var Building = await _context.UserBuildings.FirstOrDefaultAsync(x => x.BuildingId == request.BuildingId && x.BuildingType == request.BuildingType && x.UserId == User.Id && x.BuildFinishTime < DateTime.Now && !x.isFinished);
            if (Building == null) return NotFound("No building found.");
            Building.isFinished = true;
            Building.BuildingLevel += 1;
            if (Building.BuildingType == StaticDetails.ProductionBuilding)
            {
                var FarmTask = await _context.UserBuildingTasks.FirstOrDefaultAsync(x => x.UserBuildingId == Building.Id && x.UserId == User.Id);
                var ProductionConfg = await _context.ProductionBuildings.FirstOrDefaultAsync(x => x.Id == Building.BuildingId);
                var TaskTime = ProductionConfg.TaskTimeBase + (ProductionConfg.TaskTimeStep * Building.BuildingLevel);
                if (FarmTask == null)
                {
                    var NewTask = new UserBuildingTask
                    {
                        TaskType = StaticDetails.ProductionBuilding,
                        UserBuildingId = Building.Id,
                        UserId = User.Id,
                        TaskEndTime = DateTime.UtcNow.AddMinutes(TaskTime)
                    };
                    _context.UserBuildingTasks.Add(NewTask);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    FarmTask.LastTaskEndTime = FarmTask.TaskEndTime;
                    FarmTask.TaskEndTime = DateTime.UtcNow.AddMinutes(TaskTime);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Successfuly finished building.");
        }

        [HttpPost]
        public async Task<IActionResult> StartBuilding(BuildingRequest request)
        {
            var BuildingConfig = await GetBuildingConfigAsync(request);
            if (BuildingConfig == null) return NotFound("Something went wrong :(");
            var res = await InsertBuildingAsync(BuildingConfig.Id, BuildingConfig, request.BuildingType);
            if (res)
            {
                return Ok($"Started building.");
            }
            else
            {
                return BadRequest($"You cant do that!");
            }
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelBuilding(BuildingRequest request)
        {
            var BuildingConfig = await GetBuildingConfigAsync(request);
            if (BuildingConfig == null) return NotFound("Something went wrong :(");
            var User = await _utilityService.GetUser();
            var Building = await _context.UserBuildings.FirstOrDefaultAsync(x => x.BuildingId == request.BuildingId && x.BuildingType == request.BuildingType && x.UserId == User.Id && !x.isFinished);

            if (Building != null)
            {
                if (Building.BuildingLevel == 0)
                {
                    _context.UserBuildings.Remove(Building);
                    await _context.SaveChangesAsync();
                    return Ok($"Canceled.");
                }
                Building.isFinished = true;
                Building.BuildFinishTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok($"Canceled.");
            }
            else
            {
                return BadRequest($"You cant do that!");
            }
        }

        private async Task<dynamic> GetBuildingConfigAsync(BuildingRequest request)
        {
            if (request.BuildingType.Equals(StaticDetails.UnitBuilding)) // Unit
            {
                var BuildingConfig = await _context.UnitBuildings.FindAsync(request.BuildingId);
                return BuildingConfig;
            }
            else if (request.BuildingType.Equals(StaticDetails.ProductionBuilding)) // Production
            {
                var BuildingConfig = await _context.ProductionBuildings.FindAsync(request.BuildingId);
                return BuildingConfig;
            }
            else if (request.BuildingType.Equals(StaticDetails.UtilityBuilding)) // Utility
            {
                var BuildingConfig = await _context.UtilityBuildings.FindAsync(request.BuildingId);
                return BuildingConfig;
            }
            return null;
        }


        private async Task<bool> InsertBuildingAsync(int BuildingId, dynamic BuildingConfig, string BuildingType)
        {
            var user = await _utilityService.GetUser();
            var UserBulding = await _context.UserBuildings.FirstOrDefaultAsync(x => x.BuildingId == BuildingId && x.BuildingType == BuildingType && x.UserId == user.Id);
            var cost = BuildingConfig.BananaCostBase + (BuildingConfig.BananaCostStep * (UserBulding != null ? UserBulding.BuildingLevel : 0));
            if (user.Bananas < cost) return false;
            if (UserBulding == null)
            {
                //start new building
                UserBulding = new UserBuilding
                {
                    BuildingId = BuildingConfig.Id,
                    BuildingType = BuildingType,
                    UserId = user.Id,
                    BuildStartTime = DateTime.UtcNow,
                    BuildFinishTime = DateTime.UtcNow.AddMinutes(BuildingConfig.BuildTimeBase + BuildingConfig.BuildTimeStep)
                };
                _context.UserBuildings.Add(UserBulding);
                user.Bananas -= cost;
                await _context.SaveChangesAsync();

                return true;

            }
            else if (UserBulding.BuildingLevel < BuildingConfig.MaxLevel)
            {
                //upgrade building
                if (UserBulding.isFinished == false) return false;
                UserBulding.isFinished = false;
                UserBulding.BuildStartTime = DateTime.UtcNow;
                UserBulding.BuildFinishTime = DateTime.UtcNow.AddMinutes(BuildingConfig.BuildTimeBase + (BuildingConfig.BuildTimeStep * UserBulding.BuildingLevel));
                user.Bananas -= cost;
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                //return max level exception
                return false;
            }
        }

        [HttpPut("endtask")]
        public async Task<IActionResult> EndTask(BuildingRequest request)
        {
            var config = await GetBuildingConfigAsync(request);
            var User = await _utilityService.GetUser();
            var UserBuilding = await _context.UserBuildings.FirstOrDefaultAsync(x => x.UserId == User.Id && x.BuildingId == request.BuildingId && x.BuildingType == request.BuildingType);
            var UserTask = await _context.UserBuildingTasks.FirstOrDefaultAsync(x => x.UserBuildingId == UserBuilding.Id && x.UserId == User.Id && x.TaskType == request.BuildingType);

            if (UserTask.TaskEndTime < DateTime.UtcNow)
            {
                var TaskTime = config.TaskTimeBase + (config.TaskTimeStep * UserBuilding.BuildingLevel);
                UserTask.LastTaskEndTime = DateTime.UtcNow;
                UserTask.TaskEndTime = DateTime.UtcNow.AddMinutes(TaskTime);
                var Amount = config.BananaCountBase + (config.BananaCountStep * UserBuilding.BuildingLevel);
                User.Bananas += Amount;
                await _context.SaveChangesAsync();
                return Ok($"Collected {Amount} Bananas!");
            }
            else
            {
                var TaskTime = config.TaskTimeBase + (config.TaskTimeStep * UserBuilding.BuildingLevel);
                var MinutesPassed = DateTime.UtcNow.Subtract(UserTask.LastTaskEndTime).TotalMinutes;
                var maxTime = config.TaskTimeBase + (config.TaskTimeStep * UserBuilding.BuildingLevel);
                var maxBananas = config.BananaCountBase + (config.BananaCountStep * UserBuilding.BuildingLevel);
                var Percent = MinutesPassed / maxTime;
                var Amount = Convert.ToInt32(Math.Floor(maxBananas * Percent));

                UserTask.LastTaskEndTime = DateTime.UtcNow;
                UserTask.TaskEndTime = DateTime.UtcNow.AddMinutes(TaskTime);

                User.Bananas += Amount;
                await _context.SaveChangesAsync();
                return Ok($"Collected {Amount} Bananas!");
            }
        }

        [HttpGet("getmaxbananas/{buildingid}/{level}")]
        public async Task<IActionResult> GetMaxBananas(int BuildingId, int Level)
        {
            var User = await _utilityService.GetUser();
            var config = await _context.ProductionBuildings.FindAsync(BuildingId);
            var UsersBuilding = await _context.UserBuildings.FirstOrDefaultAsync(x => x.BuildingId == config.Id && x.UserId == User.Id && x.BuildingType == StaticDetails.ProductionBuilding);
            if (UsersBuilding == null) return NotFound();
            var response = config.BananaCountBase + (config.BananaCountStep * UsersBuilding.BuildingLevel);

            return Ok(response);
        }

        [HttpGet("gettask/{buildingid}")]
        public async Task<IActionResult> GetMaxBananas(int BuildingId)
        {
            var User = await _utilityService.GetUser();
            var BuildingTask = await _context.UserBuildingTasks.FirstOrDefaultAsync(x => x.UserId == User.Id && x.TaskType == StaticDetails.ProductionBuilding && x.UserBuildingId == BuildingId);
            return Ok(BuildingTask);
        }
    }
}
