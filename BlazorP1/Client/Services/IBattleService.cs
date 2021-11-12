using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public interface IBattleService
    {
        Task<BattleResult> StartBattle(int opponentId);
        Task<BattleResult> StartMission(string MissionLevel);

        BattleResult LastBattle { get; set; }

        IList<BattleHistoryEntry> History {  get; set; }
        int SinceLastLogin { get; set; }

        Task GetHistory();
        Task GetLastHistory();

        Task LastLoginUpdate();

    }
}
