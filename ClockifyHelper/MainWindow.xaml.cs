using ClockifyHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClockifyHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly HttpClient _client;

		private User _user;
		private List<Workspace> _workspaces;
		private Workspace _selectedWorkspace;
		private readonly string _executable;

		public MainWindow(HttpClient client)
		{
			_client = client;
			_user = null;
			_workspaces = new List<Workspace>();
			_executable = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

			InitializeComponent();

			ValidateAPIKeyButton.IsDefault = true;

			resetForm();

			if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastConfiguredAPIKey))
			{
				APIKeyTextBox.Text = Properties.Settings.Default.LastConfiguredAPIKey;
			}

			ValidateAPIKeyButton.Click += ValidateAPIKeyButton_Click;
			SelectWorkspaceButton.Click += SelectWorkspaceButton_Click;
			CopyClockOutButton.Click += CopyClockOutButton_Click;
			CopyClockInButton.Click += CopyClockInButton_Click;
		}

		private async void ValidateAPIKeyButton_Click(object sender, RoutedEventArgs e)
		{
			resetForm();

			// Check Input
			if (string.IsNullOrWhiteSpace(APIKeyTextBox.Text))
			{
				MessageBox.Show("Please enter an API Key!");
				return;
			}
			
			try
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				
				// API object for this request
				var api = new Clockify(_client);

				// Get User
				_user = await api.GetGeneric<User>(APIKeyTextBox.Text, "user");
				if (_user == null)
				{
					throw new ApplicationException("Failed to get the user for an unknown reason.");
				}

				UsernameTextBox.Text = _user.Email;

				// Save the API Key for next time this settings dialog is opened
				Properties.Settings.Default.LastConfiguredAPIKey = APIKeyTextBox.Text;
				Properties.Settings.Default.Save();

				// Get Workspaces
				_workspaces = await api.GetGeneric<List<Workspace>>(APIKeyTextBox.Text, "workspaces");
				if (_workspaces == null)
				{
					throw new ApplicationException("Failed to get the workspaces for an unknown reason.");
				}

				WorkspaceComboBox.Items.Clear();
				WorkspaceComboBox.ItemsSource = _workspaces;
				WorkspaceComboBox.DisplayMemberPath = "Name";
				WorkspaceComboBox.Items.Refresh();

				WorkspaceComboBox.IsEnabled = true;
				WorkspaceComboBox.IsReadOnly = false;
				SelectWorkspaceButton.IsEnabled = true;
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
			}
		}

		private void SelectWorkspaceButton_Click(object sender, RoutedEventArgs e)
		{
			_selectedWorkspace = (WorkspaceComboBox.SelectedItem as Workspace);

			ClockInTextBox.Text = _executable +
				" --action=start --api-key=" +
				APIKeyTextBox.Text +
				" --workspace=" +
				_selectedWorkspace.Id;
			ClockInTextBox.ToolTip = ClockInTextBox.Text;

			ClockOutTextBox.Text = _executable +
				" --action=stop --api-key=" +
				APIKeyTextBox.Text +
				" --workspace=" +
				_selectedWorkspace.Id;
			ClockOutTextBox.ToolTip = ClockOutTextBox.ToolTip;

			ClockInTextBox.IsEnabled = true;
			ClockOutTextBox.IsEnabled = true;
			CopyClockInButton.IsEnabled = true;
			CopyClockOutButton.IsEnabled = true;
		}

		private void CopyClockInButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Clipboard.SetText(ClockInTextBox.Text);
			CopyClockInButton.Content = "Copied";
			Task.Run(() =>
			{
				Thread.Sleep(3000); // delay
				Dispatcher.Invoke(() =>
				{
					CopyClockInButton.Content = "Copy";
				});
			});
		}

		private void CopyClockOutButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Clipboard.SetText(ClockOutTextBox.Text);
			CopyClockOutButton.Content = "Copied";
			Task.Run(() =>
			{
				Thread.Sleep(3000); // delay
				Dispatcher.Invoke(() =>
				{
					CopyClockOutButton.Content = "Copy";
				});
			});
		}

		private void resetForm()
		{
			// Reset form state
			UsernameTextBox.Text = "";
			WorkspaceComboBox.Items.Clear();
			WorkspaceComboBox.Items.Add("Validate your API Key first");
			WorkspaceComboBox.SelectedIndex = 0;
			WorkspaceComboBox.Items.Refresh();
			WorkspaceComboBox.IsEnabled = false;
			WorkspaceComboBox.IsReadOnly = true;
			SelectWorkspaceButton.IsEnabled = false;
			ClockInTextBox.Text = "";
			ClockInTextBox.ToolTip = "";
			ClockInTextBox.IsEnabled = false;
			CopyClockInButton.IsEnabled = false;
			ClockOutTextBox.Text = "";
			ClockOutTextBox.ToolTip = "";
			ClockOutTextBox.IsEnabled = false;
			CopyClockOutButton.IsEnabled = false;
		}
	}
}
