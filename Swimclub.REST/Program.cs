using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Swimclub.REST
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			InitialiseDatabase(host);
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseUrls($"https://{GetLocalIPAddress()}:443", $"https://localhost:443");
				});
		public static void InitialiseDatabase(IHost _host)
		{
			using (var scope = _host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					Demo.SeedData.InitialiseAsync(services).Wait();
				} catch(Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred seeding the database.");
				}
			}
		}

		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
	}

	public sealed class LaunchFlags
	{
		private bool newHost = false;
		private bool newHost_changed = false;
		public bool NewHost { get { return newHost; } set { setNewHost(value); } }

		private static LaunchFlags instance = null;
		private static readonly object padlock = new object();

		LaunchFlags()
		{
		}

		private void setNewHost(bool value)
		{
			if (newHost_changed)
				throw new UnauthorizedAccessException("Swimclub.REST>>>LaunchFlags>>>Cannot set the value of NewHost multiple times");
			else
			{
				newHost = value;
				newHost_changed = true;
			}
		} 

		public static LaunchFlags Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new LaunchFlags();
					}
					return instance;
				}
			}
		}
	}
}
