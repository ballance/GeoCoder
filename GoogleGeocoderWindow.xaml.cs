using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

namespace GeocodingHarness
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class GoogleGeocoderWindow : Window
	{
		
		public GoogleGeocoderWindow()
		{
			InitializeComponent();
		}

		private async void GeocodeThis_Click(object sender, RoutedEventArgs e)
		{
			var inputAddressText = InputAddress.Text;

			GeocodeThis.IsEnabled = false;
			try
			{
				await GeocodeWithGoogleAsync(inputAddressText);
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

		private async Task GeocodeWithGoogleAsync(string inputAddressText)
		{
			Status.Text = "Querying Google...";
			
			var foundLocation = await QueryGoogleGeocoderAsync(inputAddressText);
			if (!foundLocation.IsValid)
			{
				Status.Text = "Unable to geocode.  Check that your config has a valid Google Maps API key.";
				return;
			}
			Latitude.Text = foundLocation.geometry.location.lat.ToString();
			Longitude.Text = foundLocation.geometry.location.lng.ToString();
			GeocodedAddress.Text = foundLocation.formatted_address;
			PlaceId.Text = foundLocation.place_id;
			FormattedAddress.Text = foundLocation.formatted_address;
			AddressComponents.Text = GenerateAddressComponentText(foundLocation.address_components);
			LocationBounds.Text = GenerateBounds(foundLocation.geometry.bounds);
		}

		private string GenerateBounds(Bounds bounds)
		{
			if (bounds == null)
			{
				return "bounds are unavailable";
			}
			return $"SW:{bounds.southwest.lat},{bounds.southwest.lng}{Environment.NewLine}NE:{bounds.northeast.lat},{bounds.northeast.lng}";
		}

		private string GenerateAddressComponentText(List<AddressComponent> address_components)
		{
			var sb = new StringBuilder();
			if (address_components.Any())
			{
				foreach (var address in address_components)
				{
					var label = address.types.FirstOrDefault();
					var value = address.long_name;
					sb.Append($"{label}: {value}{Environment.NewLine}");
				}
			}
			return sb.ToString();
		}

		private async Task<GoogleResult> QueryGoogleGeocoderAsync(string inputAddress)
		{
			var geocoder = new GoogleGeocoder();
			geocoder.GeocodeAddress(inputAddress);
			var geocodedLocation = await geocoder.GetJsonObjectFromGeocodeResultAsync();

			Status.Text = geocodedLocation.status;

			var foundLocation = new GoogleResult
			{
				IsValid = false
			};
			if (geocodedLocation.status != "REQUEST_DENIED" && geocodedLocation.results.Any())
			{
				foundLocation = geocodedLocation.results.First(); // This is horrid, but for now...
				foundLocation.IsValid = true;
			}
			return foundLocation;
		}
	}
}

