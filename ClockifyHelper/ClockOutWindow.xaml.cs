using ClockifyHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for ClockOutWindow.xaml
	/// </summary>
	public partial class ClockOutWindow : Window
	{
		private readonly HttpClient _client;
		private readonly CommandLineOptions _opts;

		private User _user;
		private List<Workspace> _workspaces;
		private TimeEntry _timeEntry;
		private List<Project> _projects;
		private List<KeyValuePair<string, Project>> _filteredProjects;
		private List<Tag> _tags;

		private TypeAssistant _projectSearchEvent;

		private Regex _alphaNumericRegex = new Regex("[^A-Z0-9]", RegexOptions.Compiled);

		public ClockOutWindow(HttpClient client, CommandLineOptions opts)
		{
			_client = client;
			_opts = opts;
			_projectSearchEvent = new TypeAssistant();

			InitializeComponent();

			this.ContentRendered += ClockOutWindow_ContentRendered;
			_projectSearchEvent.Idled += _projectSearchEvent_Idled;
			ProjectTextBox.TextChanged += ProjectTextBox_TextChanged;
			ClockOutButton.IsDefault = true;
			ClockOutButton.Click += ClockOutButton_Click;
		}

		private async void ClockOutButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				this.IsEnabled = false;

				if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
				{
					throw new ApplicationException("You must provide a Description!");
				}

				if (ProjectResultsListBox.SelectedItem == null)
				{
					throw new ApplicationException("You must select a Project!");
				}

				if (TagComboBox.SelectedItems.Count == 0)
				{
					throw new ApplicationException("You must select at least one tag!");
				}

				var selectedProject = ((KeyValuePair<string, Project>)ProjectResultsListBox.SelectedItem);
				var selectedTags = new List<Tag>();
				foreach (var t in TagComboBox.SelectedItems)
				{
					selectedTags.Add((Tag)t);
				}

				var api = new Clockify(_client);

				var input = new Clockify.ClockOutDti();
				input.start = _timeEntry.TimeInterval.Start;
				input.end = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
				input.description = DescriptionTextBox.Text;
				input.billable = (BillableCheckbox.IsChecked ??= false) ? "true" : "false";
				input.projectId = selectedProject.Value.Id;
				input.tagIds = selectedTags.Select(x => x.Id).ToList();

				var result = await api.ClockOut(_opts.APIKey, _opts.Workspace, _timeEntry.Id, input);
				if (result == null)
				{
					throw new ApplicationException("Unknown response from Clockify when trying to Clock Out! Please use the Website to check your timesheet!");
				}

				this.Close();
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
				this.IsEnabled = true;
			}
		}

		private void _projectSearchEvent_Idled(object sender, EventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				if (!string.IsNullOrWhiteSpace(ProjectTextBox.Text))
				{
					
					_filteredProjects = new List<KeyValuePair<string, Project>>();
					foreach (var p in _projects)
					{
						string displayString = p.Name + " - " + p.ClientName;

						if (_alphaNumericRegex.Replace(
							displayString.ToUpper(),"").Contains(
								_alphaNumericRegex.Replace(
									ProjectTextBox.Text.ToUpper(), "")))
						{
							_filteredProjects.Add(new KeyValuePair<string, Project>(displayString, p));
						}
					}
					_filteredProjects = _filteredProjects.OrderBy(x => x.Value.ClientName).ThenBy(x => x.Value.Name).ToList();
					ProjectResultsListBox.ItemsSource = _filteredProjects;
					ProjectResultsListBox.DisplayMemberPath = "Key";
					ProjectResultsListBox.Items.Refresh();
				}
			});
		}

		private void ProjectTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_projectSearchEvent.TextChanged();
		}

		private async void ClockOutWindow_ContentRendered(object sender, EventArgs e)
		{
			try
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				this.IsEnabled = false;

				var api = new Clockify(_client);

				_user = await api.GetGeneric<User>(_opts.APIKey, "user");
				if (_user == null)
				{
					throw new ApplicationException("Failed to get the user for an unknown reason.");
				}
				UsernameTextBox.Text = _user.Email;

				_workspaces = await api.GetGeneric<List<Workspace>>(_opts.APIKey, "workspaces");
				if (_workspaces == null)
				{
					throw new ApplicationException("Failed to get the workspaces for an unknown reason");
				}

				var workspace = _workspaces.FirstOrDefault(x => x.Id == _opts.Workspace);
				if (workspace == null)
				{
					throw new ApplicationException("You do not have access to a workspace with that ID");
				}
				WorkspaceTextBox.Text = workspace.Name;

				var timeEntries = await api.GetGeneric<List<TimeEntry>>(_opts.APIKey, "workspaces/" + _opts.Workspace + "/user/" + _user.Id + "/time-entries?in-progress=true");
				if (timeEntries == null)
				{
					throw new ApplicationException("Failed to get the time entries for an unknown reason");
				}
				if (timeEntries.Count == 0)
				{
					throw new ApplicationException("You are not clocked in right now!");
				}
				else if (timeEntries.Count > 1)
				{
					throw new ApplicationException("You are clocked in multiple times right now in the same workspace. I don\'t know how to handle that!");
				}
				_timeEntry = timeEntries.First();
				ClockStartedTextBox.Text = _timeEntry.TimeInterval.Start;

				_projects = await api.GetGeneric<List<Project>>(_opts.APIKey, "workspaces/" + _opts.Workspace + "/projects?archived=false");
				if (_projects == null)
				{
					throw new ApplicationException("Failed to get the projects for an unknown reason");
				}
				_filteredProjects = new List<KeyValuePair<string, Project>>();
				foreach (var p in _projects)
				{
					_filteredProjects.Add(new KeyValuePair<string, Project>(p.Name + " - " + p.ClientName, p));
				}
				_filteredProjects = _filteredProjects.OrderBy(x => x.Value.ClientName).ThenBy(x => x.Value.Name).ToList();
				ProjectResultsListBox.ItemsSource = _filteredProjects;
				ProjectResultsListBox.DisplayMemberPath = "Key";
				ProjectResultsListBox.Items.Refresh();

				_tags = await api.GetGeneric<List<Tag>>(_opts.APIKey, "workspaces/" + _opts.Workspace + "/tags?archived=false");
				if (_tags == null)
				{
					throw new ApplicationException("Failed to get the tags for any unknown reason");
				}
				_tags = _tags.OrderBy(x => x.Name).ToList();
				TagComboBox.ItemsSource = _tags;
				TagComboBox.DisplayMemberPath = "Name";
				TagComboBox.Items.Refresh();
			}
			catch (Exception ex)
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
				MessageBox.Show("ERROR: " + ex.Message);
				this.Close();
			}
			finally
			{
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
				this.IsEnabled = true;
				DescriptionTextBox.Focus();
			}
		}

		public class TypeAssistant
		{
			public event EventHandler Idled = delegate { };
			public int WaitingMilliSeconds { get; set; }
			System.Threading.Timer waitingTimer;

			public TypeAssistant(int waitingMilliSeconds = 600)
			{
				WaitingMilliSeconds = waitingMilliSeconds;
				waitingTimer = new System.Threading.Timer(p =>
				{
					Idled(this, EventArgs.Empty);
				});
			}
			public void TextChanged()
			{
				waitingTimer.Change(WaitingMilliSeconds, System.Threading.Timeout.Infinite);
			}
		}
	}
}
