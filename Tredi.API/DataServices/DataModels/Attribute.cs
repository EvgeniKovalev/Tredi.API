using Newtonsoft.Json;
using System.Security.AccessControl;
using Tredi.API.Models;

namespace Tredi.API.DataServices.DataModels
{
	public class Attribute
	{
		[JsonProperty(PropertyName = "PartitionKey")]
		public string PartitionKey { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		public string Name { get; set; }
		public string Label { get; set; }
		public string ValueStr { get; set; }
		public bool IsActive { get; set; } = true;
		public AttributeTypeEnum AttributeType { get; set; } = AttributeTypeEnum.TEXTHTML;

		public Attribute() { }

		public Attribute(AttributeDto attributeDto)
		{
			PartitionKey = "Attribute";
			Id = Guid.NewGuid().ToString();

			if (attributeDto != null)
			{
				if (!string.IsNullOrWhiteSpace(attributeDto.Id))
				{
					Id = attributeDto.Id;
				}

				if (!string.IsNullOrWhiteSpace(attributeDto.PartitionKey))
				{
					PartitionKey = attributeDto.PartitionKey;
				}

				Name = attributeDto.Name;
				Label = attributeDto.Label;
				AttributeType = attributeDto.AttributeType;
				ValueStr = attributeDto.ValueStr;
			}
		}
	}
}
