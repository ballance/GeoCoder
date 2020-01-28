using System.Collections.Generic;

namespace GeocodingHarness
{
	public class EntityLink
	{
		public int createdBy { get; set; }
		public int updatedBy { get; set; }
		public int rmsEventId { get; set; }
		public int departmentId { get; set; }
		public int locationId { get; set; }
		public int entityId { get; set; }
		public object subdivision1AttrId { get; set; }
		public object subdivision2AttrId { get; set; }
		public object subdivision3AttrId { get; set; }
		public object subdivision4AttrId { get; set; }
		public List<object> shapefileProperties { get; set; }
		public List<object> dispatchAreas { get; set; }
	}

	public class Datum
	{
		public string type { get; set; }
		public object departmentId { get; set; }
		public string country { get; set; }
		public string postalCode { get; set; }
		public string adminArea1 { get; set; }
		public string locality { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string streetAddress { get; set; }
		public string crossStreet1 { get; set; }
		public string streetAddressAlias { get; set; }
		public string crossStreet1Alias { get; set; }
		public object id { get; set; }
		public List<EntityLink> entityLinks { get; set; }
		public bool areCoordinatesVerified { get; set; }
		public string crossStreet2 { get; set; }
		public string crossStreet2Alias { get; set; }
		public string neighborhood { get; set; }
	}

	public class CustomAddressResults
	{
		public List<Datum> data { get; set; }
		public bool success { get; set; }
	}
}
