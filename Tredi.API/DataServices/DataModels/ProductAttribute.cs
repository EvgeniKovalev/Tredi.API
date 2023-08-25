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

		public string Name { get; set; }
		public string Label { get; set; }
		public bool IsActive { get; set; }
		public AttributeTypeEnum AttributeType { get; set; } = AttributeTypeEnum.TEXTHTML;

		public ProductAttribute() { }

		public ProductAttribute(AttributeDto attributeDto)
		{
			PartitionKey = "Attribute";
			Id = Guid.NewGuid().ToString();
			Name = attributeDto.Name;
			Label = attributeDto.Label;
			IsActive = true;
			AttributeType = attributeDto.AttributeType;
		}
	}
}
