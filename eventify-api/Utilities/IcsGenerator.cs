using eventify_api.Models;
using System.Text;

namespace eventify_api.Utilities
{
    public static class IcsGenerator
    {
        public static string GenerateIcs(EventDetails eventDetails)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"SUMMARY:{eventDetails.Summary}");
            sb.AppendLine($"LOCATION:{eventDetails.Location}");
            sb.AppendLine($"DESCRIPTION:{eventDetails.Description}");
            sb.AppendLine($"DTSTART:{eventDetails.StartTime.Replace("-", "").Replace(":", "")}");
            sb.AppendLine($"DTEND:{eventDetails.EndTime.Replace("-", "").Replace(":", "")}");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            // log the generated ICS string for debugging (remove this in production)
            Console.WriteLine($"Generated ICS: {sb.ToString()}");

            return sb.ToString();
        }
    }
}
