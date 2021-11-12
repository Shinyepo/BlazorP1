using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
    }
}
