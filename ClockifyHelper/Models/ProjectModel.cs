using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyHelper.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ProjectMembership
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("hourlyRate")]
        public string HourlyRate { get; set; }

        [JsonProperty("costRate")]
        public string CostRate { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }

        [JsonProperty("membershipType")]
        public string MembershipType { get; set; }

        [JsonProperty("membershipStatus")]
        public string MembershipStatus { get; set; }
    }

    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hourlyRate")]
        public string HourlyRate { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("workspaceId")]
        public string WorkspaceId { get; set; }

        [JsonProperty("billable")]
        public string Billable { get; set; }

        [JsonProperty("memberships")]
        public List<ProjectMembership> Memberships { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("estimate")]
        public string Estimate { get; set; }

        [JsonProperty("archived")]
        public string Archived { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("costRate")]
        public string CostRate { get; set; }

        [JsonProperty("timeEstimate")]
        public string TimeEstimate { get; set; }

        [JsonProperty("budgetEstimate")]
        public string BudgetEstimate { get; set; }

        [JsonProperty("public")]
        public string Public { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }
    }


}
