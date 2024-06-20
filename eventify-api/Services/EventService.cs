using eventify_api.Interfaces;
using eventify_api.Models;
using Newtonsoft.Json;

namespace eventify_api.Services
{
    public class EventService : IEventService
    {
        private readonly IOpenAIService _openAIService;

        public EventService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public async Task<EventDetails> GetEventDetailsAsync(string inputText)
        {
            var eventDetailsJson = await _openAIService.GetEventDetailsAsync(inputText);

            // Log the event details JSON for debugging (remove this in production)
            Console.WriteLine($"Event Details JSON: {eventDetailsJson}");

            // Clean the JSON content
            string cleanedJson = CleanJsonContent(eventDetailsJson);

            // Deserialize the cleaned JSON into the EventDetails object
            EventDetails eventDetails;
            try
            {
                eventDetails = JsonConvert.DeserializeObject<EventDetails>(cleanedJson);
                // Log the deserialized event details for debugging (remove this in production)
                Console.WriteLine($"Event Details: {JsonConvert.SerializeObject(eventDetails, Formatting.Indented)}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize Event Details JSON: {ex.Message}");
                throw;
            }

            return eventDetails;
        }

        private string CleanJsonContent(string jsonContent)
        {
            // Remove the "```json\n" prefix and the "\n```" suffix
            if (jsonContent.StartsWith("```json\n") && jsonContent.EndsWith("\n```"))
            {
                jsonContent = jsonContent.Substring(8, jsonContent.Length - 11);
            }
            return jsonContent;
        }
    }
}