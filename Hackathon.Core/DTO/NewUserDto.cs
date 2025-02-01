using Hackathon.Core.Models;

namespace Hackathon.Core.DTO
{
    public class NewUserDto
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public string CRM { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
