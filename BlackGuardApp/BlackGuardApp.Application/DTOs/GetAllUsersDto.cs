using BlackGuardApp.Domain.Enum;

namespace BlackGuardApp.Application.DTOs
{
    public class GetAllUsersDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRoles Roles { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPasswordSet { get; set; }
    }
}
