using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ClockifyHelper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static readonly HttpClient httpClient = new HttpClient();
		
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var parser = new Parser(config =>
			{
				config.AutoHelp = false;
				config.AutoVersion = false;
			});

			var parserResult = parser.ParseArguments<CommandLineOptions>(e.Args);

			var helpText = HelpText.AutoBuild(parserResult, h =>
			{
				h.AdditionalNewLineAfterOption = false;
				h.Heading = new HeadingInfo("Clockify Helper", "1.0.0");
				h.Copyright = new CopyrightInfo("Andrew Haskell");
				h.AutoHelp = false;
				h.AutoVersion = false;

				return h;
			}, e => e);

			var retcode = parserResult.MapResult((CommandLineOptions opts) =>
				{
					try
					{
						if (opts.Help)
						{
							MessageBox.Show(helpText);
							return 0;
						}

						if (opts.Version)
						{
							MessageBox.Show(helpText.Heading + "\r\n" + helpText.Copyright);
							return 0;
						}

						switch (opts.Action)
						{
							case "start":
								if (string.IsNullOrWhiteSpace(opts.APIKey) ||
									string.IsNullOrWhiteSpace(opts.Workspace))
								{
									MessageBox.Show(helpText);
									return -1;
								}

								ClockInWindow clockInWindow = new ClockInWindow(httpClient, opts);
								clockInWindow.ShowDialog();
								return 0;
							case "stop":
								if (string.IsNullOrWhiteSpace(opts.APIKey) ||
									string.IsNullOrWhiteSpace(opts.Workspace))
								{
									MessageBox.Show(helpText);
									return -1;
								}

								ClockOutWindow clockOutWindow = new ClockOutWindow(httpClient, opts);
								clockOutWindow.ShowDialog();
								return 0;
							default:
								MainWindow mainWindow = new MainWindow(httpClient);
								mainWindow.ShowDialog();
								return 0;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Unhandled Exception Occured: " + ex.Message);
						return -3;
					}
				},
				errs => {
					MessageBox.Show(helpText);
					return -1;
				});

			Application.Current.Shutdown(retcode);
		}
	}
}
