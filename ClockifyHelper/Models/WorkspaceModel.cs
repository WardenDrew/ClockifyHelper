using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyHelper.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class WorkspaceHourlyRate
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class WorkspaceMembership
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("hourlyRate")]
        public object HourlyRate { get; set; }

        [JsonProperty("costRate")]
        public object CostRate { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }

        [JsonProperty("membershipType")]
        public string MembershipType { get; set; }

        [JsonProperty("membershipStatus")]
        public string MembershipStatus { get; set; }
    }

    public class WorkspaceRound
    {
        [JsonProperty("round")]
        public string Round { get; set; }

        [JsonProperty("minutes")]
        public string Minutes { get; set; }
    }

    public class WorkspaceSettings
    {
        [JsonProperty("timeRoundingInReports")]
        public string TimeRoundingInReports { get; set; }

        [JsonProperty("onlyAdminsSeeBillableRates")]
        public string OnlyAdminsSeeBillableRates { get; set; }

        [JsonProperty("onlyAdminsCreateProject")]
        public string OnlyAdminsCreateProject { get; set; }

        [JsonProperty("onlyAdminsSeeDashboard")]
        public string OnlyAdminsSeeDashboard { get; set; }

        [JsonProperty("defaultBillableProjects")]
        public string DefaultBillableProjects { get; set; }

        [JsonProperty("lockTimeEntries")]
        public string LockTimeEntries { get; set; }

        [JsonProperty("round")]
        public WorkspaceRound Round { get; set; }

        [JsonProperty("projectFavorites")]
        public string ProjectFavorites { get; set; }

        [JsonProperty("canSeeTimeSheet")]
        public string CanSeeTimeSheet { get; set; }

        [JsonProperty("canSeeTracker")]
        public string CanSeeTracker { get; set; }

        [JsonProperty("projectPickerSpecialFilter")]
        public string ProjectPickerSpecialFilter { get; set; }

        [JsonProperty("forceProjects")]
        public string ForceProjects { get; set; }

        [JsonProperty("forceTasks")]
        public string ForceTasks { get; set; }

        [JsonProperty("forceTags")]
        public string ForceTags { get; set; }

        [JsonProperty("forceDescription")]
        public string ForceDescription { get; set; }

        [JsonProperty("onlyAdminsSeeAllTimeEntries")]
        public string OnlyAdminsSeeAllTimeEntries { get; set; }

        [JsonProperty("onlyAdminsSeePublicProjectsEntries")]
        public string OnlyAdminsSeePublicProjectsEntries { get; set; }

        [JsonProperty("trackTimeDownToSecond")]
        public string TrackTimeDownToSecond { get; set; }

        [JsonProperty("projectGroupingLabel")]
        public string ProjectGroupingLabel { get; set; }

        [JsonProperty("adminOnlyPages")]
        public List<object> AdminOnlyPages { get; set; }

        [JsonProperty("automaticLock")]
        public object AutomaticLock { get; set; }

        [JsonProperty("onlyAdminsCreateTag")]
        public string OnlyAdminsCreateTag { get; set; }

        [JsonProperty("onlyAdminsCreateTask")]
        public string OnlyAdminsCreateTask { get; set; }

        [JsonProperty("timeTrackingMode")]
        public string TimeTrackingMode { get; set; }

        [JsonProperty("isProjectPublicByDefault")]
        public string IsProjectPublicByDefault { get; set; }
    }

    public class Workspace
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hourlyRate")]
        public WorkspaceHourlyRate HourlyRate { get; set; }

        [JsonProperty("memberships")]
        public List<WorkspaceMembership> Memberships { get; set; }

        [JsonProperty("workspaceSettings")]
        public WorkspaceSettings WorkspaceSettings { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("featureSubscriptionType")]
        public object FeatureSubscriptionType { get; set; }
    }


}
