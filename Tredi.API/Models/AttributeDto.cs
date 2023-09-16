using Tredi.API.DataServices.DataModels;

namespace Tredi.API.Models
{
	public class AttributeDto
	{
		public string? PartitionKey { get; set; }
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Label { get; set; }
		public string? ValueStr { get; set; }
		public AttributeTypeEnum AttributeType { get; set; } = AttributeTypeEnum.TEXTHTML;

		public AttributeDto() { }

		public AttributeDto(DataServices.DataModels.Attribute attribute)
		{
			if (attribute != null)
			{
				PartitionKey = attribute.PartitionKey;
				Id = attribute.Id;
				Name = attribute.Name;
				Label = attribute.Label;
				AttributeType = attribute.AttributeType;
				ValueStr = attribute.ValueStr;
			}
		}

		public AttributeDto(ProductAttribute productAttribute)
		{
			if (productAttribute != null)
			{
				Id = productAttribute.AttributeId;
				ValueStr = productAttribute.Value;
			}
		}
	}
}
