using Newtonsoft.Json;
using Tredi.API.Models;

namespace Tredi.API.DataServices.DataModels
{
	public class ProductAttribute
	{
		[JsonProperty(PropertyName = "PartitionKey")]
		public string PartitionKey { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		public string ProductId { get; set; }
		public string AttributeId { get; set; }
		public bool IsActive { get; set; } = true;
		public string? Value { get; set; }

		public ProductAttribute() { }

		public ProductAttribute(string productId, AttributeDto attributeDto)
		{
			PartitionKey = "ProductAttribute";
			Id = Guid.NewGuid().ToString();
			ProductId = productId;
			IsActive = true;

			if (attributeDto != null)
			{
				AttributeId = attributeDto.Id;
				Value = attributeDto.ValueStr;
			}
		}
	}
}
