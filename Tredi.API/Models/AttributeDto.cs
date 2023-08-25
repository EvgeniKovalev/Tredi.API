using Tredi.API.DataServices.DataModels;

namespace Tredi.API.Models
{
	public class AttributeDto
	{
		public string Name { get; set; }
		public string Label { get; set; }
		public AttributeTypeEnum AttributeType { get; set; } = AttributeTypeEnum.TEXTHTML;

		public AttributeDto() { }

		public AttributeDto(DataServices.DataModels.ProductAttribute attribute)
		{
			if (attribute != null)
			{
				Name = attribute.Name;
				Label = attribute.Label;
				AttributeType = attribute.AttributeType;
			}
		}
	}
}
