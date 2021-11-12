using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Services
{
    public interface IUtilityService
    {
        Task<User> GetUser();

    }
}
