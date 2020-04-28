using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace GeocodingHarness
{
	public partial class Menu : Window
	{
		
		public Menu()
		{
			InitializeComponent();
		}

		private void Geocode_Click(object sender, RoutedEventArgs e)
		{
			Button theButton = sender as Button;
			string url = theButton.Tag.ToString();

			//this.navFrame.Navigate(new Uri(url, UriKind.Relative));
		}

		private void ReverseGeocode_Click(object sender, RoutedEventArgs e)
		{
			Button theButton = sender as Button;
			string url = theButton.Tag.ToString();

			//this.navFrame.Navigate(new Uri(url, UriKind.Relative));
		}
	}
}

