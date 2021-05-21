using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyHelper.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class TimeEntryTimeInterval
    {
        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }
    }

    public class TimeEntryCustomField
    {
        [JsonProperty("customFieldId")]
        public string CustomFieldId { get; set; }

        [JsonProperty("timeEntryId")]
        public string TimeEntryId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TimeEntry
    {
        [JsonProperty("billable")]
        public string Billable { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isLocked")]
        public string IsLocked { get; set; }

        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("tagIds")]
        public List<string> TagIds { get; set; }

        [JsonProperty("taskId")]
        public string TaskId { get; set; }

        [JsonProperty("timeInterval")]
        public TimeEntryTimeInterval TimeInterval { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("workspaceId")]
        public string WorkspaceId { get; set; }

        [JsonProperty("customFields")]
        public List<TimeEntryCustomField> CustomFields { get; set; }
    }


}
