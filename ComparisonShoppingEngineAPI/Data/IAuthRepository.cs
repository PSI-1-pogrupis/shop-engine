using ComparisonShoppingEngineAPI.Data.Models;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string userName, string userEmail);

    }
}
