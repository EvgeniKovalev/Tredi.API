using Tredi.API.DataServices.DataModels;

namespace Tredi.API.Models
{
	public class AttributeDto
	{
		public string? PartitionKey { get; set; }
		public string? Id { get; set; }
		public string Name { get; set; }
		public AttributeTypeEnum AttributeType { get; set; } = AttributeTypeEnum.TEXTHTML;

		public AttributeDto() { }

		public AttributeDto(ProductAttribute attribute)
		{
			if (attribute != null)
			{
				PartitionKey = attribute.PartitionKey;
				Id = attribute.Id;
				Name = attribute.Name;
				AttributeType = attribute.AttributeType;
			}
		}
	}
}
