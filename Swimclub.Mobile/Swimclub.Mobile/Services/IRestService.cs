using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Swimclub.Models;
using Xamarin.Forms;

namespace Swimclub.Mobile.Services
{
	public interface IRestService
	{
		string AuthToken { get; }
		string Role { get; }
		string ApiUrl { set; }
		/// <summary>
		/// Logs the user in, sets the <see cref="AuthToken"/> and <see cref="Role"/>
		/// </summary>
		/// <param name="_loginModel"></param>
		/// <returns>200 = OK, 401 = Unauthorised, 503 = Server down</returns>
		Task<AuthResponse> LoginAsync(Login _loginModel);
		Task<AllStudentsResponse> GetAllStudentsAsync();
		Task<AllGradesResponse> GetAllGradesAsync();
		Task<CreateStudentResponse> CreateStudent(Models.Student student);
		Task<ModifyStudentResponse> ModifyStudent(Models.Student student);
		Task<RegistrationResponse> RegisterUser(Models.Register register);
		void ClearClient();

	}

	public class RestService : IRestService
	{
		private string api_url;
		public string ApiUrl
		{
			set { SetURL(value); }
		}
		//private WinHttpHandler handler;
		private HttpClient client;

		private string authToken;
		public string AuthToken { get { return authToken; } }
		private string role;
		public string Role { get { return role; } }

		private IConfigurationService config;
		public RestService()
		{
			//handler = new WinHttpHandler();
			//HttpClientHandler httpHandler = new HttpClientHandler();
			//httpHandler.ServerCertificateCustomValidationCallback
			config = DependencyService.Get<IConfigurationService>();
			//api_url = $"https://{config.ConfigValues["apiIP"]}:{config.ConfigValues["apiPORT"]}";

			HttpClientHandler h = new System.Net.Http.HttpClientHandler();

			h.ServerCertificateCustomValidationCallback =
				(message, certificate, chain, sslPolicyErrors) => true;
			client = new HttpClient(h);
			
		}

		public void ClearClient()
		{
			authToken = "";
			role = "";
		}

		private void SetURL(string IP)
		{
			//api_url = $"https://{IP}:{config.ConfigValues["apiPORT"]}";
			api_url = $"https://{IP}";
		}

		public async Task<AuthResponse> LoginAsync(Login _loginModel)
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
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				//Swimclub.Models.standard.Collection<Student> students = JsonConvert.DeserializeObject<Swimclub.Models.standard.Collection<Student>>(responseBody);
				Swimclub.Models.AuthResponse res = JsonConvert.DeserializeObject<Swimclub.Models.AuthResponse>(responseBody);
				if (res.Success)
				{
					role = res.Role;
					authToken = res.Token;
				}
				
				return res;
			}
			else
				return new AuthResponse() { Success = false, Error = ApiError.TimeOutResponse() };
		
		}

		public async Task<AllStudentsResponse> GetAllStudentsAsync()
		{
			if (authToken == null)
				return new AllStudentsResponse() { Success = false , Error = ApiError.NotLoggedInError() };

			Uri uri = new Uri(String.Format("{0}/{1}/{2}", api_url, "students", "all"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = uri,
				
			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}",authToken));
			//var response = await client.SendAsync(request).ConfigureAwait(false);
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));
			//if (response.StatusCode != System.Net.HttpStatusCode.OK)
			//{
			//	return new AllStudentsReturn() { Success = false };
			//}
			//else
			//{
			//	var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			//	Swimclub.Models.standard.Collection<Student> students = JsonConvert.DeserializeObject<Swimclub.Models.standard.Collection<Student>>(responseBody);
			//	return new AllStudentsReturn() { Success = false, Students = students.values};
			//}

			if (response.Wait(TimeSpan.FromSeconds(30)))
			{
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				//Swimclub.Models.standard.Collection<Student> students = JsonConvert.DeserializeObject<Swimclub.Models.standard.Collection<Student>>(responseBody);
				Swimclub.Models.AllStudentsResponse res = JsonConvert.DeserializeObject<Swimclub.Models.AllStudentsResponse>(responseBody);
				return res;
			}
			else
			{
				return new AllStudentsResponse() { Success = false, Error = ApiError.TimeOutResponse() };
			}
		}

		public async Task<AllGradesResponse> GetAllGradesAsync()
		{
			if (authToken == null)
				return new AllGradesResponse() { Success = false, Error = ApiError.NotLoggedInError()};

			Uri uri = new Uri(String.Format("{0}/{1}/{2}", api_url, "grades", "all"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = uri,

			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}", authToken));
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));

			if (response.Wait(TimeSpan.FromSeconds(40)))
			{
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				Swimclub.Models.AllGradesResponse res = JsonConvert.DeserializeObject<Swimclub.Models.AllGradesResponse>(responseBody);
				return res;

			}
			else
			{
				return new AllGradesResponse() { Success = false, Error = ApiError.TimeOutResponse()};
			}
		}

		public async Task<ModifyStudentResponse> ModifyStudent(Models.Student student)
		{
			if (authToken == null)
				return new ModifyStudentResponse() { Success = false, Error = ApiError.NotLoggedInError()};

			Uri uri = new Uri(String.Format("{0}/{1}", api_url, "modify"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json")

			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}", authToken));
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));

			if (response.Wait(TimeSpan.FromSeconds(40)))
			{
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				Swimclub.Models.ModifyStudentResponse res = JsonConvert.DeserializeObject<Swimclub.Models.ModifyStudentResponse>(responseBody);
				return res;

			}
			else
			{
				return new ModifyStudentResponse() { Success = false, Error = ApiError.TimeOutResponse()};
			}
		}

		public async Task<CreateStudentResponse> CreateStudent(Models.Student student)
		{
			if (authToken == null)
				return new CreateStudentResponse() { Success = false, Error = ApiError.NotLoggedInError()};

			Uri uri = new Uri(String.Format("{0}/{1}", api_url, "add"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json")

			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}", authToken));
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));

			if (response.Wait(TimeSpan.FromSeconds(40)))
			{
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				Swimclub.Models.CreateStudentResponse res = JsonConvert.DeserializeObject<Swimclub.Models.CreateStudentResponse>(responseBody);
				return res;

			}
			else
			{
				return new CreateStudentResponse() { Success = false, Error = ApiError.TimeOutResponse()};
			}
		}



		public async Task<RegistrationResponse> RegisterUser(Models.Register register)
		{
			if (authToken == null)
				return new RegistrationResponse() { Success = false, Error = ApiError.NotLoggedInError()};

			Uri uri = new Uri(String.Format("{0}/{1}/{2}", api_url, "users", "register"));
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json")

			};
			request.Headers.Add("Authorization", String.Format("Bearer {0}", authToken));
			Task<HttpResponseMessage> response = Task.Run(() => client.SendAsync(request));

			if (response.Wait(TimeSpan.FromSeconds(40)))
			{
				var responseBody = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
				RegistrationResponse res = JsonConvert.DeserializeObject<Swimclub.Models.RegistrationResponse>(responseBody);
				return res;

			}
			else
			{
				return new RegistrationResponse() { Success = false, Error = ApiError.TimeOutResponse()};
			}
		}
	}
}
