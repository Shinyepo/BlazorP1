using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public interface ILeaderboardService
    {
        IList<UserStatistic> Leaderboard { get; set; }
        int MyUserId { get; set; }
        Task GetLeaderboard();
        Task<int> GetMyRank();
    }
}
