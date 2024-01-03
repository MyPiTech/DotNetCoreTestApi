using System.ComponentModel.DataAnnotations;

namespace Test.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Notes { get; set; }
        //one-to-many
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
