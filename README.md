# Eventify API

Eventify API is a web API that extracts event details from text input using OpenAI's GPT-3.5 model and returns them as a JSON object. It also generates an ICS file that can be used to add events to calendar applications. You can find the client-side code for this API in [this](https://github.com/whuang214/eventify-client) repo.

### Features
- Extracts event details from provided text.
- Returns event details as a JSON object.
- Generates ICS files for easy calendar integration.
- Utilizes OpenAI's GPT-4o model for natural language processing.

### Prerequisites
- [.NET 8.0 SDK or later](https://dotnet.microsoft.com/en-us/download)
- [OpenAI API key](https://platform.openai.com/api-keys)

### Endpoints

#### POST /api/createEvent
Extracts event details from text input and returns a ICS file.

##### Request Body (Example)
```json
{
  "inputText": "Company All-Hands Meeting\n\nDate: Thursday, July 15, 2024\nTime: 9:00 AM - 10:30 AM\n\nLocation: Main Auditorium, 1st Floor\n\nDescription: Join us for our quarterly all-hands meeting where we will discuss the company's performance, upcoming projects, and answer any questions you may have. Coffee and pastries will be served.\n\nAdd to Calendar\n\nContact: HR Department, (123) 456-7890"
}
```
##### Response (ICS File)
```vbnet
BEGIN:VCALENDAR
VERSION:2.0
BEGIN:VEVENT
SUMMARY:Company All-Hands Meeting
LOCATION:Main Auditorium, 1st Floor
DESCRIPTION:Join us for our quarterly all-hands meeting where we will discuss the company's performance, upcoming projects, and answer any questions you may have. Coffee and pastries will be served.
DTSTART:20240715T090000
DTEND:20240715T103000
END:VEVENT
END:VCALENDAR
```

### Configuration
- Make sure to add your OpenAI API key to the User Secrets/`secrets.json`. You can do so using the following commands:
```bash
dotnet user-secrets init
dotnet user-secrets set "ApiSettings:OpenAPI_Key" "<your-api-key>"
```
