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
	public partial class MainWindow : Window
	{
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void GeocodeThis_Click(object sender, RoutedEventArgs e)
		{
			GeocodeThis.IsEnabled = false;
			try
			{
				Status.Text = "Querying...";
				var inputAddress = InputAddress.Text;

				var foundLocation = await QueryGoogleGeocoderAsync(inputAddress);
				Latitude.Text = foundLocation.geometry.location.lat.ToString();
				Longitude.Text = foundLocation.geometry.location.lng.ToString();
				GeocodedAddress.Text = foundLocation.formatted_address;
				PlaceId.Text = foundLocation.place_id;
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

		private async Task<Result> QueryGoogleGeocoderAsync(string inputAddress)
		{
			var geocoder = new Geocoder();
			geocoder.GeocodeAddress(inputAddress);
			var geocodedLocation = await geocoder.GetJsonObjectFromGeocodeResultAsync();

			Status.Text = geocodedLocation.status;

			var foundLocation = geocodedLocation.results.First(); // This is horrid, but for now...
			return foundLocation;
		}
	}
}

