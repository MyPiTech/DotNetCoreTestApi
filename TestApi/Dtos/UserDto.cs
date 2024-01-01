using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Notes { get; set; }

        public IList<EventDto> ?Events { get; set; }
    }
}
