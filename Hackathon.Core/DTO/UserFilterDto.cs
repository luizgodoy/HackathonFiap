using Hackathon.Core.Models;

namespace Hackathon.Core.DTO;

public class UserFilterDto
{
    public Role? Role { get; set; }
    public string Specialty { get; set; } = "";
}