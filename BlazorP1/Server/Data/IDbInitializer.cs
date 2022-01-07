using System.Threading.Tasks;

namespace BlazorP1.Server.Data
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}
