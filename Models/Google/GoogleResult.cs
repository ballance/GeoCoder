using System.Collections.Generic;

namespace GeocodingHarness
{
	public class GoogleResult
	{
		public List<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
		public Geometry geometry { get; set; }
		public string place_id { get; set; }
		public List<string> types { get; set; }

		public bool IsValid { get; set; }
	}
}
