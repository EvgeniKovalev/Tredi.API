using Tredi.API.DataServices.DataModels;

namespace Tredi.API.Models
{
	public class ProductDto
	{
		public string? PartitionKey { get; set; }
		public string? Id { get; set; }
		public string? Name { get; set; }
		public List<AttributeDto> Attributes { get; set; } = new List<AttributeDto>();
		public ProductDto() { }

		public ProductDto(Product product, List<ProductAttribute> productAttributes)
		{
			if (product != null)
			{
				PartitionKey = product.PartitionKey;
				Id = product.Id;
				Name = product.Name;

				if (productAttributes != null)
				{
					foreach (var productAttribute in productAttributes)
					{
						Attributes.Add(item: new AttributeDto(productAttribute));
					}
				}
			}
		}
	}
}
