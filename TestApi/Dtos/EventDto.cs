namespace TestApi.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public int ?UserId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime ?Start { get; set; }
        public int ?Duration { get; set; }
    }
}
