using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models.Auth
{
    [PrimaryKey(nameof(Id))]
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PassWordHash { get; set; }
    }

    public record UserDTO
    {
        public required string UserName { get; set; }
        public required string PassWord { get; set; }
    }
}
