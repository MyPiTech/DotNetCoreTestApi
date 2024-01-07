using PGTest.Data;
using System.ComponentModel.DataAnnotations;

namespace Test.Data
{
    public class User : Entity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Notes { get; set; }
        //one-to-many
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
