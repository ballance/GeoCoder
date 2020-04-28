using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Geocoding.Models.Google;

namespace GeocodingHarness
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			InputAddress.Text = ConfigurationManager.AppSettings["starting-location"];
		}

		private async void GeocodeThis_Click(object sender, RoutedEventArgs e)
		{
			GeocodeThis.IsEnabled = false;
			try
			{
				Status.Text = "Querying...";
				var inputAddress = InputAddress.Text;

				var foundLocations = await QueryGoogleGeocoderAsync(inputAddress);
				if (!foundLocations.Any(x => x.IsValid))
				{
					Status.Text = "Unable to geocode or no locations found.  You might want to check that your config has a valid Google Maps API key.";
					return;
				}

				LocationsFound.Text = foundLocations.Count().ToString();
				var foundLocation = foundLocations.First();
				Latitude.Text = foundLocation.geometry.location.lat.ToString();
				Longitude.Text = foundLocation.geometry.location.lng.ToString();
				GeocodedAddress.Text = foundLocation.formatted_address;
				PlaceId.Text = foundLocation.place_id;
				FormattedAddress.Text = foundLocation.formatted_address;
				AddressComponents.Text = GenerateAddressComponentText(foundLocation.address_components);
				LocationBounds.Text =  GenerateBounds(foundLocation.geometry.bounds);
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

		private async Task<List<Result>> QueryGoogleGeocoderAsync(string inputAddress)
		{
			var geocoder = new GoogleGeocoder();
			geocoder.GeocodeAddress(inputAddress);
			var geocodedLocation = await geocoder.GetJsonObjectFromGeocodeResultAsync();

			Status.Text = geocodedLocation.status;

			List<Result> foundLocations = new List<Result>();
			
			if (geocodedLocation.status != "REQUEST_DENIED" && geocodedLocation.results.Any())
			{
				foundLocations = geocodedLocation.results; // This is horrid, but for now...
														   //foundLocation.IsValid = true;
				foreach (var result in foundLocations)
				{
					result.IsValid = true;
				}
			}
			return foundLocations;
		}
	}
}

