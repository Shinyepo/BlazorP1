using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public interface IBananaService
    {
        event Action OnChange;

        int Bananas { get; set; }
        Task EatBananas(int amount);
        Task GrowBananas(int amount);

        Task GetBananas();
    }
}
