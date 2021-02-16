using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swimclub.Mobile.Services
{
	interface IConfigurationService
	{
		Dictionary<string, string> ConfigValues { get; }
	}

	public class ConfigurationService : IConfigurationService
	{
		Dictionary<string, string> config;
		private bool loaded = false;
		public Dictionary<string,string> ConfigValues { get { return config; } }

		public ConfigurationService()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));
			var file = assembly.GetManifestResourceStream(resName);
			var sr = new StreamReader(file);
			var json = sr.ReadToEnd();
			config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
		}
	}
}
