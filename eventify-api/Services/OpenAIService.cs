using eventify_api.Interfaces;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eventify_api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["ApiSettings:OpenAPI_Key"];

            // Log the API key for debugging (remove this in production)
            Console.WriteLine($"API Key: {_apiKey}");
        }

        public async Task<string> GetEventDetailsAsync(string inputText)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            // Log the Authorization header for debugging (remove this in production)
            Console.WriteLine($"Authorization Header: Bearer {_apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant that extracts event details from text and returns them as JSON with fields: summary, location, description, start_time, end_time." },
                    new { role = "user", content = inputText }
                }
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            Console.WriteLine($"OpenAI API Response Status Code: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorResponse}");
            }

            response.EnsureSuccessStatusCode(); // This line will throw an exception if the response status is not successful (2xx)

            var responseBody = await response.Content.ReadAsStringAsync();

            // Extract the JSON content string from the response body
            var responseObject = JObject.Parse(responseBody);
            var messageContent = responseObject["choices"]?[0]?["message"]?["content"]?.ToString();

            // Clean the JSON string
            messageContent = messageContent.Trim('`', '\n').Trim();

            return messageContent;
        }
    }
}