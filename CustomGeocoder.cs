using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace GeocodingHarness
{
	public class CustomGeocoder
	{

		private string _customApiKey;
		private string _baseUrl;
		private static string _configFileName = "custom-dev.config";
		private static string _baseUrlFileName = "base-url-custom-dev.config";
		private string _rawResult = String.Empty;

		public CustomGeocoder()
		{
			SetApiKeyFromConfig();
			SetBaseUrlFromConfig();
		}

		private void SetBaseUrlFromConfig()
		{
			_baseUrl = ConfigurationManager.AppSettings["custom-api-base-url"];
			if (string.IsNullOrWhiteSpace(_baseUrl))
			{
				// Try loading from file
				if (File.Exists(_baseUrlFileName))
				{
					_baseUrl = File.ReadAllText(_baseUrlFileName);
				}
			}
		}

		private void SetApiKeyFromConfig()
		{
			_customApiKey = ConfigurationManager.AppSettings["custom-api-key"];
			if (string.IsNullOrWhiteSpace(_customApiKey))
			{
				// Try loading from file
				if (File.Exists(_configFileName))
				{
					_customApiKey = File.ReadAllText(_configFileName);
				}
			}
		}

		public bool IsComplete { get; private set; } = false;
		


		public async void GeocodeAddress(string inputAddress)
		{
			WebClient webClient = new WebClient();
			webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GecodeAddressCompleted);

			var username = _customApiKey;
			var password = "x-api-token";
			var baseUrl = _baseUrl;

			string credentials = Convert.ToBase64String(
				ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

			string encodedInputAddress = HttpUtility.UrlEncode(inputAddress);
			webClient.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";

			var customGeocoderUrl = new Uri($"{baseUrl}{encodedInputAddress}");

			webClient.DownloadStringAsync(customGeocoderUrl);
		}

		private void GecodeAddressCompleted(object sender, DownloadStringCompletedEventArgs e)
		{

			IsComplete = true;
			_rawResult = e.Result;
		}

		private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			IsComplete = true;
			_rawResult = e.Result;
		}

		public async Task<CustomAddressResults> GetJsonObjectFromGeocodeResultAsync()
		{
			while (!IsComplete)
			{
				await Task.Delay(10);
			}
			var jsonResult = JsonConvert.DeserializeObject<CustomAddressResults>(_rawResult);
			IsComplete = false;
			return jsonResult;
		}
	}
}
