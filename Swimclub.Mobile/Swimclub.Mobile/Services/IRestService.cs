﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Swimclub.Models;

namespace Swimclub.Mobile.Services
{
	public interface IRestService
	{
		string AuthToken { get; }
		string Role { get; }
		/// <summary>
		/// Logs the user in, sets the <see cref="AuthToken"/> and <see cref="Role"/>
		/// </summary>
		/// <param name="_loginModel"></param>
		/// <returns>200 = OK, 401 = Unauthorised, 503 = Server down</returns>
		Task<int> LoginAsync(Login _loginModel);
		Task<Student[]> GetAllStudentsAsync();

	}

	public class RestService : IRestService
	{
		private static string api_url = "https://192.168.1.225:5001";
		//private WinHttpHandler handler;
		private HttpClient client;

		private string authToken;
		public string AuthToken { get { return authToken; } }
		private string role;
		public string Role { get { return role; } }

		public RestService()
		{
			//handler = new WinHttpHandler();
			//HttpClientHandler httpHandler = new HttpClientHandler();
			//httpHandler.ServerCertificateCustomValidationCallback
			HttpClientHandler h = new System.Net.Http.HttpClientHandler();

			h.ServerCertificateCustomValidationCallback =
				(message, certificate, chain, sslPolicyErrors) => true;
			client = new HttpClient(h);
			
		}

		public async Task<int> LoginAsync(Login _loginModel)
		{
			Uri uri = new Uri(String.Format("{0}/{1}/{2}", api_url, "auth", "login"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = new StringContent(JsonConvert.SerializeObject(_loginModel), Encoding.UTF8, "application/json")
			};
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));

			if (response.Wait(TimeSpan.FromSeconds(20)))
			{
				if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return 401;
				}
				else
				{
					var responseBody = await response.Result.Content.ReadAsStringAsync();
					AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseBody);
					authToken = authResponse.Token;
					role = authResponse.Role;
					return 200;
				}
			}
			else
				return 503;
		
		}

		public async Task<Student[]> GetAllStudentsAsync()
		{
			if (authToken == null)
				throw new UnauthorizedAccessException("No bearer token detected");

			Uri uri = new Uri(String.Format("{0}/{1}/{2}", api_url, "students", "all"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = uri,
				
			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}",authToken));
			var response = await client.SendAsync(request).ConfigureAwait(false);
			if(response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				return null;
			}
			else
			{
				var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				Swimclub.Models.standard.Collection<Student> students = JsonConvert.DeserializeObject<Swimclub.Models.standard.Collection<Student>>(responseBody);
				return students.values;
			}
		}
	}
}
