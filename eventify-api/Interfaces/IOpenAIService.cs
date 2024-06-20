namespace eventify_api.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GetEventDetailsAsync(string inputText);
    }
}
