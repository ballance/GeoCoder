using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace GeocodingHarness
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class CustomGeocoderWindow : Window
	{

		public CustomGeocoderWindow()
		{
			InitializeComponent();
		}

		private async void GeocodeThis_Click(object sender, RoutedEventArgs e)
		{
			var inputAddressText = InputAddress.Text;
			
			GeocodeThis.IsEnabled = false;
			try
			{	
				await GeocodeWithCustomAsync(inputAddressText);	
			}
			catch (Exception ex)
			{
				Status.Text = ex.Message;
			}
			finally
			{
				GeocodeThis.IsEnabled = true;
			}
		}

		
		private async Task GeocodeWithCustomAsync(string inputAddressText)
		{
			Status.Text = "Querying Mark43...";

			CustomAddressResults foundLocation;
			try
			{
				foundLocation = await QueryCustomGeocoderAsync(inputAddressText);
				if (foundLocation.success != true)
				{
					Status.Text = "Unable to geocode.";
					return;
				}
			}
			catch (Exception ex)
			{
				Status.Text = "Error encountered while attempting to geocode.";
				return;
			}

			var firstLocation = foundLocation.data.First();
			Latitude.Text = firstLocation.latitude.ToString();
			Longitude.Text = firstLocation.longitude.ToString();

			GeocodedAddress.Text = firstLocation.streetAddress;
			PlaceId.Text = "n/a";
			Status.Text = "Ok";
		}

		private string GenerateBounds(Bounds bounds)
		{
			if (bounds == null)
			{
				return "bounds are unavailable";
			}
			return $"SW:{bounds.southwest.lat},{bounds.southwest.lng}{Environment.NewLine}NE:{bounds.northeast.lat},{bounds.northeast.lng}";
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
					
		}

		private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
		{
			
		}

		private async Task<CustomAddressResults> QueryCustomGeocoderAsync(string inputAddress)
		{
			var geocoder = new CustomGeocoder();
			geocoder.GeocodeAddress(inputAddress);
			var geocodedLocation = await geocoder.GetJsonObjectFromGeocodeResultAsync();

			Status.Text = geocodedLocation.success ? "OK" : "Error";

			var foundLocation = new Datum();
			if (geocodedLocation.success && geocodedLocation.data.Any())
			{
				foundLocation = geocodedLocation.data.First(); // This is horrid, but for now...
			}
			return geocodedLocation;
		}
	}
}

