using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Image;
using ComparisonShoppingEngineAPI.DTOs.User;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserUpdate>> Update(UserUpdateDto userUpdate);
        Task<ServiceResponse<GetImageDto>> GetProfileImage();
        Task<ServiceResponse<string>> GetGender();
    }
}
