using EventScheduler.Data;
using EventScheduler.Models.Entities;
using EventScheduler.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Repository.ServiceRepo
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task AddEventAsync(Event model)
        {
            await _context.Events.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event model)
        {
            _context.Events.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
