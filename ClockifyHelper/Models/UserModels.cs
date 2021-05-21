using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyHelper.Models
{
    public class User
    {
        [JsonProperty("activeWorkspace")]
        public string ActiveWorkspace { get; set; }

        [JsonProperty("defaultWorkspace")]
        public string DefaultWorkspace { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("memberships")]
        public List<UserMembership> Memberships { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("settings")]
        public UserSettings Settings { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class UserHourlyRate
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class UserCostRate
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class UserMembership
    {
        [JsonProperty("hourlyRate")]
        public UserHourlyRate HourlyRate { get; set; }

        [JsonProperty("costRate")]
        public UserCostRate CostRate { get; set; }

        [JsonProperty("membershipStatus")]
        public string MembershipStatus { get; set; }

        [JsonProperty("membershipType")]
        public string MembershipType { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }

    public class UserSummaryReportSettings
    {
        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("subgroup")]
        public string Subgroup { get; set; }
    }

    public class UserSettings
    {
        [JsonProperty("collapseAllProjectLists")]
        public string CollapseAllProjectLists { get; set; }

        [JsonProperty("dashboardPinToTop")]
        public string DashboardPinToTop { get; set; }

        [JsonProperty("dashboardSelection")]
        public string DashboardSelection { get; set; }

        [JsonProperty("dashboardViewType")]
        public string DashboardViewType { get; set; }

        [JsonProperty("dateFormat")]
        public string DateFormat { get; set; }

        [JsonProperty("isCompactViewOn")]
        public string IsCompactViewOn { get; set; }

        [JsonProperty("longRunning")]
        public string LongRunning { get; set; }

        [JsonProperty("projectListCollapse")]
        public object ProjectListCollapse { get; set; }

        [JsonProperty("sendNewsletter")]
        public string SendNewsletter { get; set; }

        [JsonProperty("summaryReportSettings")]
        public UserSummaryReportSettings SummaryReportSettings { get; set; }

        [JsonProperty("timeFormat")]
        public string TimeFormat { get; set; }

        [JsonProperty("timeTrackingManual")]
        public string TimeTrackingManual { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("weekStart")]
        public string WeekStart { get; set; }

        [JsonProperty("weeklyUpdates")]
        public string WeeklyUpdates { get; set; }
    }

    


}
