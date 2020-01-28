using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GeocodingHarness
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
		}

		private void Go_Click(object sender, RoutedEventArgs e)
		{
			var geocoderSelected = GeocoderSelector.Text;
			switch (geocoderSelected)
			{
				case "Google":
					{
						GoogleGeocoderWindow googleWindow = new GoogleGeocoderWindow();
						googleWindow.Show();
						this.Close();
						break;
					}
				case "Custom":
					{
						CustomGeocoderWindow customWindow = new CustomGeocoderWindow();
						customWindow.Show();
						this.Close();
						break;
					}
				default:
					{
						break;
					}
			}
		}
	}
}
