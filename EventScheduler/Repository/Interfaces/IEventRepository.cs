using EventScheduler.Models.Entities;

namespace EventScheduler.Repository.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task AddEventAsync(Event model);
        Task UpdateEventAsync(Event model);
        Task DeleteEventAsync(int id);
    }
}
