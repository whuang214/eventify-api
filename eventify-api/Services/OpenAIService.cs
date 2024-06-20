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
                    new { role = "system", content = @"
You are an assistant that extracts event details from text and returns them as JSON. 
The JSON should include the following fields: 
- summary: A brief title of the event
- location: The location where the event will take place
- description: A detailed description of the event
- start_time: The start time of the event in ISO 8601 format (YYYY-MM-DDTHH:MM:SS)
- end_time: The end time of the event in ISO 8601 format (YYYY-MM-DDTHH:MM:SS)

Here are some examples of the expected output:

Example 1:
Input: 'Company All-Hands Meeting\n\nDate: Thursday, July 15, 2024\nTime: 9:00 AM - 10:30 AM\n\nLocation: Main Auditorium, 1st Floor\n\nDescription: Join us for our quarterly all-hands meeting where we will discuss the company's performance, upcoming projects, and answer any questions you may have. Coffee and pastries will be served.\n\nAdd to Calendar\n\nContact: HR Department, (123) 456-7890'
Output: {
  ""summary"": ""Company All-Hands Meeting"",
  ""location"": ""Main Auditorium, 1st Floor"",
  ""description"": ""Join us for our quarterly all-hands meeting where we will discuss the company's performance, upcoming projects, and answer any questions you may have. Coffee and pastries will be served."",
  ""start_time"": ""2024-07-15T09:00:00"",
  ""end_time"": ""2024-07-15T10:30:00""
}

Example 2:
Input: 'Yoga Class with Instructor Mia\n\nDate: Monday, August 2, 2024\nTime: 6:30 PM - 7:30 PM\n\nLocation: Fitness Center, Room 101\n\nDescription: Relax and unwind with our evening yoga session led by our experienced instructor, Mia. All levels are welcome. Please bring your own mat and water bottle.\n\nAdd to Calendar\n\nContact: Wellness Center, (987) 654-3210'
Output: {
  ""summary"": ""Yoga Class with Instructor Mia"",
  ""location"": ""Fitness Center, Room 101"",
  ""description"": ""Relax and unwind with our evening yoga session led by our experienced instructor, Mia. All levels are welcome. Please bring your own mat and water bottle."",
  ""start_time"": ""2024-08-02T18:30:00"",
  ""end_time"": ""2024-08-02T19:30:00""
}

Please extract the event details from the following text and return them as JSON.
" },
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