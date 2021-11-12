using BlazorP1.Client.Pages;
using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password, int startUnitId);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> EmailTaken(string email);
        Task<bool> UsernameTaken(string username);
    }
}
