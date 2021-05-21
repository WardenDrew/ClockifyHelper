using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClockifyHelper
{
	/// <summary>
	/// Interaction logic for ClockInWindow.xaml
	/// </summary>
	public partial class ClockInWindow : Window
	{
		private readonly HttpClient _client; 
		private readonly CommandLineOptions _opts;
		
		public ClockInWindow(HttpClient client, CommandLineOptions opts)
		{
			_client = client;
			_opts = opts;
			InitializeComponent();
			this.ContentRendered += ClockInWindow_ContentRendered1;
			AbortButton.Click += AbortButton_Click;
		}

		private void AbortButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Please Check Clockify as a time entry may or may not have been created!");
			Application.Current.Shutdown(0);
			this.Close();
		}

		private async void ClockInWindow_ContentRendered1(object sender, EventArgs e)
		{
			this.Activate();

			try
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				var api = new Clockify(_client);
				var timeEntry = await api.ClockIn(_opts.APIKey, _opts.Workspace);
				if (timeEntry == null)
				{
					throw new ApplicationException("Failed to clock in for an unknown reason.");
				}
			}
			catch (Exception ex)
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
				MessageBox.Show("ERROR: " + ex.Message);
				return;
			}
			finally
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
				this.Close();
			}
		}
	}
}
