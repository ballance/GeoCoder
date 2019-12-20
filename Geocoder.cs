using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeocodingHarness
{
	public class Geocoder
	{
		private static string configFileName = "dev.config";
		public Geocoder()
		{
			_googleApiKey = ConfigurationManager.AppSettings["google-api-key"];
			if (string.IsNullOrWhiteSpace(_googleApiKey))
			{
				// Try loading from file
				if (File.Exists(configFileName))
				{
					_googleApiKey = File.ReadAllText(configFileName);
				}
			}
		}
		private string _rawResult;

		public bool IsComplete { get; private set; } = false;

		private string _googleApiKey;
		public void GeocodeAddress(string inputAddress)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCompleted);

				var googleUri = new Uri(string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={1}&address={0}&sensor=false",
					Uri.EscapeDataString(inputAddress), _googleApiKey));
				
				webClient.DownloadStringAsync(googleUri);

			}
			catch (Exception ex)
			{
				throw new ApplicationException($"An error occurred.  {ex.Message}");
			}
		}

		private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			IsComplete = true;
			_rawResult = e.Result;
		}

		public async Task<GoogleResulto> GetJsonObjectFromGeocodeResultAsync()
		{
			while (!IsComplete)
			{
				await Task.Delay(10);
			}
			var jsonResult = JsonConvert.DeserializeObject<GoogleResulto>(_rawResult);
			IsComplete = false;
			return jsonResult;
		}
	}
}
