using System.ComponentModel.DataAnnotations;

namespace Test.Data
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Location { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public int Duration { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
