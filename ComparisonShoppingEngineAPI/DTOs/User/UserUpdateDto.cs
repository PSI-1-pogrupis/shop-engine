#nullable enable

using ComparisonShoppingEngineAPI.Data.Utilities;

namespace ComparisonShoppingEngineAPI.DTOs.User
{
    public class UserUpdateDto
    {
        public string? Image { get; set; }
        public string? ImageType { get; set; }
        // User defined new password
        public string? Password { get; set; }
        public string? Gender { get; set; }
        #nullable disable
        public string UserUpdateType { get; set; }
    }
}
