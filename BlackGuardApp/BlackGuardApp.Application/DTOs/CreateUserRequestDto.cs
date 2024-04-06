using BlackGuardApp.Domain.Enum;

namespace BlackGuardApp.Application.DTOs
{
    public class CreateUserRequest
    {
        public string EmailAddress { get; set; }
        public UserRoles[] Roles { get; set; }
    }
}
