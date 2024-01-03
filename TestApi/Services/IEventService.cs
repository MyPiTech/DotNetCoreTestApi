/*using TestApi.Dtos;

namespace TestApi.Services
{
    //Simple interface to demonstrate inversion of control.
    public interface IEventService
    {
        Task<List<EventDto>> GetAllEventsAsync(CancellationToken token);

        Task<EventDto> CreateEventAsync(CreateEventDto user, CancellationToken token);

        Task<EventDto?> GetEventAsync(int id, CancellationToken token);

        Task<bool> DeleteEventAsync(int id, CancellationToken token);

        Task<EventDto?> ReplaceEventAsync(int id, CreateEventDto user, CancellationToken token);

        Task<EventDto?> UpdateEventAsync(int id, EventDto user, CancellationToken token);
    }
}
*/