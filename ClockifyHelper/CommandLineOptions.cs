using CommandLine;

namespace ClockifyHelper
{
	public class CommandLineOptions
	{
		[Option(longName: "help", Hidden = true)]
		public bool Help { get; set; }

		[Option(longName: "version", Hidden = true)]
		public bool Version { get; set; }

		[Option(longName: "action", Required = false, HelpText = "The desired Action. Acceptable values are \'start\' and \'stop\'")]
		public string Action { get; set; }
		
		[Option(longName: "api-key", Required = false, HelpText = "Your Clockify API Key")]
		public string APIKey { get; set; }

		[Option(longName: "workspace", Required = false, HelpText = "The target Workspace ID")]
		public string Workspace { get; set; }
	}
}
