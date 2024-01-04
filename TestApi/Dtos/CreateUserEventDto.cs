using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class CreateUserEventDto
    {
        [Required]
        public string Title { get; set; }
        public string Location { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        [Range(1, 100)]
        public int Duration { get; set; }
    }
}
