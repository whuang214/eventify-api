using Newtonsoft.Json;

namespace eventify_api.Models
{
    public class EventDetails
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("end_time")]
        public string EndTime { get; set; }
    }

}

