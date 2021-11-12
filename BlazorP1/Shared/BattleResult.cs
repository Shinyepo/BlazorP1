using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class BattleResult
    {
        public IList<string> Log { get; set; } = new List<string>();
        public int AttackerDmgSum { get; set; }
        public int OpponentDmgSum { get; set; }
        public int Reward { get; set; }
        public bool IsVictory { get; set; }
        public int RoundsFought { get; set; }
        public int AttackerKilledUnits { get; set; } = 0;
        public int OpponentKilledUnits { get; set; } = 0;

    }
}
