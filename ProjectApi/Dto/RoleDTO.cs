using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dto
{
    public class RoleDTO
    {
        public int RoleId { get; set; } 

        public string Name { get; set; } 

        public string Description { get; set; }
    }
}
