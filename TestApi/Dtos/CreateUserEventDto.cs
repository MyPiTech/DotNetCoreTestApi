using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class CreateUserEventDto
    {
        [Required]
		[StringLength(40)]
		public string Title { get; set; }
		[StringLength(40)]
		public string Location { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        [Range(10, 100)]
        public int Duration { get; set; }
    }
}
