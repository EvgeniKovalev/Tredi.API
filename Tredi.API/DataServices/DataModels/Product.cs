using Newtonsoft.Json;
using Tredi.API.Models;

namespace Tredi.API.DataServices.DataModels
{
	public class Product
	{
		[JsonProperty(PropertyName = "PartitionKey")]
		public string PartitionKey { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; } = true;

		private List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

		public Product(ProductDto productDto)
		{
			PartitionKey = "Product";
			Id = (DateTime.Now.Ticks - new DateTime(2023, 08, 31).Ticks).ToString("x"); //sku generator

			if (productDto != null)
			{
				Name = productDto.Name;
				if (!string.IsNullOrWhiteSpace(productDto.Id))
				{
					Id = productDto.Id;
				}

				if (!string.IsNullOrWhiteSpace(productDto.PartitionKey))
				{
					PartitionKey = productDto.PartitionKey;
				}

				foreach (var attribute in productDto.Attributes)
				{
					Attributes.Add(new ProductAttribute(Id, attribute));
				}	
			}
		}

		public List<ProductAttribute> GetAttributes()
		{
			return Attributes;
		}
	}
}