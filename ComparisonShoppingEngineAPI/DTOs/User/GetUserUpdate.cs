#nullable enable

namespace ComparisonShoppingEngineAPI.DTOs.User
{
    public class GetUserUpdate
    {
        public string? ImagePath { get; set; } = null;
        public bool IsPasswordChanged { get; set; } = false;
        public bool IsDeactivated { get; set; } = false;
    }
}
