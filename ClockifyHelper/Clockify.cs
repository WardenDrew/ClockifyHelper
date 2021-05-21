using ClockifyHelper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyHelper
{
	public class Clockify
	{
		private readonly string BASE_URI = @"https://api.clockify.me/api/v1/";


		private readonly HttpClient _client;

		public Clockify(HttpClient client)
		{
			_client = client;
		}

		public async Task<TResult> GetGeneric<TResult>(string apiKey, string path)
		{
			using (var request = new HttpRequestMessage())
			{
				request.Method = HttpMethod.Get;
				request.RequestUri = new Uri(BASE_URI + path);
				request.Headers.Add("X-Api-Key", apiKey);

				var response = await _client.SendAsync(request);
				var responseContent = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException(responseContent, null, response.StatusCode);
				}

				TResult robj = JsonConvert.DeserializeObject<TResult>(responseContent);

				if (robj == null)
				{
					throw new JsonSerializationException("Deserialization of the model returned null");
				}

				return robj;
			}
		}

		public async Task<TimeEntry> ClockIn(string apiKey, string workspaceId)
		{
			using (var request = new HttpRequestMessage())
			{
				request.Method = HttpMethod.Post;
				request.RequestUri = new Uri(BASE_URI + "workspaces/" + workspaceId + "/time-entries");
				request.Headers.Add("X-Api-Key", apiKey);

				var requestjson = JsonConvert.SerializeObject(new
				{
					start = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)
				});

				var requestContent = new StringContent(requestjson, Encoding.UTF8, "application/json");
				request.Content = requestContent;

				var response = await _client.SendAsync(request);
				var responseContent = await response.Content.ReadAsStringAsync();

				var robj = JsonConvert.DeserializeObject<TimeEntry>(responseContent);

				if (robj == null)
				{
					throw new JsonSerializationException("Deserialization of the model returned null");
				}

				return robj;
			}
		}

		public async Task<TimeEntry> ClockOut(string apiKey, string workspaceId, string timeEntryId, ClockOutDti dti)
		{
			using (var request = new HttpRequestMessage())
			{
				request.Method = HttpMethod.Put;
				request.RequestUri = new Uri(BASE_URI + "workspaces/" + workspaceId + "/time-entries/" + timeEntryId);
				request.Headers.Add("X-Api-Key", apiKey);

				var requestjson = JsonConvert.SerializeObject(dti);

				var requestContent = new StringContent(requestjson, Encoding.UTF8, "application/json");
				request.Content = requestContent;

				var response = await _client.SendAsync(request);
				var responseContent = await response.Content.ReadAsStringAsync();

				var robj = JsonConvert.DeserializeObject<TimeEntry>(responseContent);

				if (robj == null)
				{
					throw new JsonSerializationException("Deserialization of the model returned null");
				}

				return robj;
			}
		}

		public class ClockOutDti
		{
			public string start { get; set; }
			public string end { get; set; }
			public string billable { get; set; }
			public string description { get; set; }
			public string projectId { get; set; }
			public List<string> tagIds { get; set; }
		}

	}
}
