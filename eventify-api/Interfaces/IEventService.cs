using eventify_api.Models;

namespace eventify_api.Interfaces
{
    public interface IEventService
    {
        Task<EventDetails> GetEventDetailsAsync(string inputText);
    }
}
