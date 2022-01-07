using Microsoft.EntityFrameworkCore;
using BlazorP1.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _db;

        public DbInitializer(DataContext db)
        {
            _db = db;
        }

        public async Task Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            

            if (_db.UnitBuildings.Count() < 1)
            {
                var Barracks = new UnitBuilding
                {
                    BuildingName = "Barracks",
                    UnitId = 1,
                    MaxLevel = 5,
                    UnitLimitStep = 2,
                    UnitLimitBase = 3,
                    BananaCostBase = 1500,
                    BananaCostStep = 760,
                    BuildTimeBase = 45,
                    BuildTimeStep = 26,
                    TaskTimeBase = 7,
                    TaskTimeStep = 4
                };
                var ArcheryRange = new UnitBuilding
                {
                    BuildingName = "Archery Range",
                    UnitId = 2,
                    MaxLevel = 5,
                    UnitLimitStep = 1,
                    UnitLimitBase = 2,
                    BananaCostBase = 4200,
                    BananaCostStep = 1230,
                    BuildTimeBase = 60,
                    BuildTimeStep = 35,
                    TaskTimeBase = 13,
                    TaskTimeStep = 6
                };
                var MageTower = new UnitBuilding
                {
                    BuildingName = "Mage Tower",
                    UnitId = 3,
                    MaxLevel = 3,
                    UnitLimitStep = 1,
                    UnitLimitBase = 1,
                    BananaCostBase = 4800,
                    BananaCostStep = 1630,
                    BuildTimeBase = 90,
                    BuildTimeStep = 45,
                    TaskTimeBase = 20,
                    TaskTimeStep = 6
                };
                var Campfire = new UnitBuilding
                {
                    BuildingName = "Campfire",
                    UnitId = 4,
                    MaxLevel = 5,
                    UnitLimitStep = 1,
                    UnitLimitBase = 1,
                    BananaCostBase = 9600,
                    BananaCostStep = 3230,
                    BuildTimeBase = 15,
                    BuildTimeStep = 45,
                    TaskTimeBase = 10,
                    TaskTimeStep = 6
                };
                await _db.UnitBuildings.AddAsync(Barracks);
                await _db.UnitBuildings.AddAsync(ArcheryRange);
                await _db.UnitBuildings.AddAsync(MageTower);
                await _db.UnitBuildings.AddAsync(Campfire);
            }

            if (_db.Units.Count() < 1)
            {
                var Knight = new Unit
                {
                    Title = "Knight",                    
                    Attack = 10,
                    Defence = 10,
                    HitPoints = 100,
                    BananaCost = 100,
                    BuildingId = 1                    
                };
                var Archer = new Unit
                {
                    Title = "Archer",
                    Attack = 15,
                    Defence = 7,
                    HitPoints = 90,
                    BananaCost = 140,
                    BuildingId = 2
                };
                var Mage = new Unit
                {
                    Title = "Mage",
                    Attack = 20,
                    Defence = 5,
                    HitPoints = 50,
                    BananaCost = 220,
                    BuildingId = 3
                };
                var Barbarian = new Unit
                {
                    Title = "Barbarian",
                    Attack = 20,
                    Defence = 4,
                    HitPoints = 120,
                    BananaCost = 700,
                    BuildingId = 4
                };
                await _db.Units.AddAsync(Knight);
                await _db.Units.AddAsync(Archer);
                await _db.Units.AddAsync(Mage);
                await _db.Units.AddAsync(Barbarian);
            }

            if (_db.UtilityBuildings.Count() < 1)
            {
                var TrainingCamp = new UtilityBuilding
                {
                    BuildingName = StaticDetails.RecruitUtilityBuilding,
                    BuildingDescription = "Lowers the cost of recruiting new units.",
                    MaxLevel = 4,
                    BananaCostBase = 4300,
                    BananaCostStep = 1300,
                    BuildTimeBase = 45,
                    BuildTimeStep = 30,
                    BonusType = "percent",
                    BonusAmountBase = 15,
                    BonusAmountStep = 5,
                };
                var ShamansHut= new UtilityBuilding
                {
                    BuildingName = StaticDetails.HealCostUtilityBuilding,
                    BuildingDescription = "Lowers the cost of healing woundend units.",
                    MaxLevel = 4,
                    BananaCostBase = 14300,
                    BananaCostStep = 3800,
                    BuildTimeBase = 60,
                    BuildTimeStep = 40,
                    BonusType = "percent",
                    BonusAmountBase = 10,
                    BonusAmountStep = 10,
                };

                await _db.UtilityBuildings.AddAsync(TrainingCamp);
                await _db.UtilityBuildings.AddAsync(ShamansHut);

            }

            if (_db.ProductionBuildings.Count() < 1)
            {
                var BananaFarm = new ProductionBuilding
                {
                    BuildingName = "Banana Farm",
                    MaxLevel = 4,
                    BananaCostBase = 3000,
                    BananaCostStep = 5000,
                    BuildTimeBase = 20,
                    BuildTimeStep = 60,
                    BananaCountBase = 1500,
                    BananaCountStep = 3000,
                    TaskTimeBase = 10,
                    TaskTimeStep = 10,
                };
                await _db.ProductionBuildings.AddAsync(BananaFarm);
            }

            if (_db.MissionLevels.Count() < 1)
            {
                var easy = new MissionLevel
                {
                    MissionName = "Easy",
                    Battles = 0,
                    Wins = 0,
                    Defeats = 0,
                };
                var medium = new MissionLevel
                {
                    MissionName = "Medium",
                    Battles = 0,
                    Wins = 0,
                    Defeats = 0,
                };
                var hard = new MissionLevel
                {
                    MissionName = "Hard",
                    Battles = 0,
                    Wins = 0,
                    Defeats = 0,
                };
                var impossible = new MissionLevel
                {
                    MissionName = "Impossible",
                    Battles = 0,
                    Wins = 0,
                    Defeats = 0,
                };
                await _db.MissionLevels.AddAsync(easy);
                await _db.MissionLevels.AddAsync(medium);
                await _db.MissionLevels.AddAsync(hard);
                await _db.MissionLevels.AddAsync(impossible);
            }

            if (_db.MissionUnits.Count() < 1)
            {
                var easy = new MissionUnits
                {
                    MissionLevelId = 1,
                    UnitId = 4,
                    HitPoints = 100,
                };
                var mediumUnits = new MissionUnits[3];
                for (int i = 0; i < mediumUnits.Length; i++)
                {
                    var temp = new MissionUnits
                    {
                        MissionLevelId = 2,
                        UnitId = 4,
                        HitPoints = 100,
                    };
                    mediumUnits[i] = temp;
                }

                var hardUnits = new MissionUnits[7];
                for (int i = 0; i < hardUnits.Length; i++)
                {
                    var temp = new MissionUnits
                    {                        
                        MissionLevelId = 3,
                        UnitId = 4,
                        HitPoints = 100,
                    };
                    hardUnits[i] = temp;
                }

                var impossibleUnits = new MissionUnits[15];
                for (int i = 0; i < impossibleUnits.Length; i++)
                {
                    var temp = new MissionUnits
                    {
                        MissionLevelId = 4,
                        UnitId = 4,
                        HitPoints = 100,
                    };
                    impossibleUnits[i] = temp;
                }

                await _db.MissionUnits.AddAsync(easy);
                await _db.MissionUnits.AddRangeAsync(mediumUnits);
                await _db.MissionUnits.AddRangeAsync(hardUnits);
                await _db.MissionUnits.AddRangeAsync(impossibleUnits);

                

            }
                var result = _db.SaveChangesAsync().Result;
                var s = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
            }

        }
    }
}
