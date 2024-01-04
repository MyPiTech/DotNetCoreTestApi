using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Notes { get; set; }

    }
}
