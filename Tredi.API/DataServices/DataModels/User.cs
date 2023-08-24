using Newtonsoft.Json;

namespace Tredi.API.DataServices.DataModels
{
	public class User
	{
		[JsonProperty(PropertyName = "PartitionKey")]
		public string PartitionKey { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		public string UserName { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }
	}
}
