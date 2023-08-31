using Microsoft.Azure.Cosmos;
using Tredi.API.DataServices.DataModels;
using Tredi.API.Models;

namespace Tredi.API.DataServices
{
	public class PimService
	{
		private CosmosClient _client;

		public PimService(CosmosClient cosmosClient)
		{
			_client = cosmosClient;
		}

		private async Task<Container> LoadContainer<T>()
		{
			Database database = await _client.CreateDatabaseIfNotExistsAsync("TrediDB");
			return await database.CreateContainerIfNotExistsAsync(typeof(T).Name, "/PartitionKey");
		}
		public async Task<List<AttributeDto>> LoadAttributes()
		{
			var container = await LoadContainer<DataModels.Attribute>();
			var attributes = container.GetItemLinqQueryable<DataModels.Attribute>(true).Where(b => b.IsActive).ToList();

			var dtoList = new List<AttributeDto>();
			if (attributes.Any())
			{
				attributes.ForEach(a => { dtoList.Add(new AttributeDto(a)); });
			}
			return dtoList;
		}

		public async Task<bool> AddAttribute(AttributeDto attributeDto)
		{
			var attribute = new DataModels.Attribute(attributeDto);
			var container = await LoadContainer<DataModels.Attribute>();
			var createdResponse = await container.CreateItemAsync(attribute, new PartitionKey(attribute.PartitionKey));
			return createdResponse.StatusCode == System.Net.HttpStatusCode.OK;
		}

		public async Task<bool> EditAttribute(AttributeDto attributeDto)
		{
			var attribute = new DataModels.Attribute(attributeDto);
			var container = await LoadContainer<DataModels.Attribute>();
			var createdResponse = await container.UpsertItemAsync(attribute, new PartitionKey(attribute.PartitionKey));
			return createdResponse.StatusCode == System.Net.HttpStatusCode.OK;
		}

		public async Task<bool> DeleteAttribute(AttributeDto attributeDto)
		{
			var attribute = new DataModels.Attribute(attributeDto);
			attribute.IsActive = false;
			var container = await LoadContainer<DataModels.Attribute>();
			var createdResponse = await container.UpsertItemAsync(attribute, new PartitionKey(attribute.PartitionKey));
			return createdResponse.StatusCode == System.Net.HttpStatusCode.OK;
		}

		public async Task<List<ProductDto>> LoadProducts()
		{
			var productContainer = await LoadContainer<Product>();
			var products = productContainer.GetItemLinqQueryable<Product>(true).Where(b => b.IsActive).ToList();
			var productAttributeContainer = await LoadContainer<ProductAttribute>();

			var dtoList = new List<ProductDto>();
			foreach (var product in products)
			{
				var productAttributes = productAttributeContainer.GetItemLinqQueryable<ProductAttribute>(true).Where(b => b.IsActive && b.ProductId == product.Id).ToList();
				dtoList.Add(new ProductDto(product, productAttributes));
			}
			return dtoList;
		}

		public async Task<bool> AddProduct(ProductDto productDto)
		{
			var product = new Product(productDto);
			var productContainer = await LoadContainer<Product>();
			await productContainer.CreateItemAsync(product, new PartitionKey(product.PartitionKey));

			await AddProductAttributes(product);
			return true;
		}

		public async Task<bool> EditProduct(ProductDto productDto)
		{
			var product = new Product(productDto);
			var container = await LoadContainer<Product>();
			await container.UpsertItemAsync(product, new PartitionKey(product.PartitionKey));

			await RemoveProductAttributes(product);
			await AddProductAttributes(product);
			return true;
		}

		public async Task<bool> DeleteProduct(ProductDto productDto)
		{
			var product = new Product(productDto);
			product.IsActive = false;
			var container = await LoadContainer<Product>();
			var createdResponse = await container.UpsertItemAsync(product, new PartitionKey(product.PartitionKey));
			return createdResponse.StatusCode == System.Net.HttpStatusCode.OK;
		}



		private async Task AddProductAttributes(Product product)
		{
			var tasks = new List<Task>();
			var productAttributeContainer = await LoadContainer<ProductAttribute>();
			foreach (var attribute in product.GetAttributes())
			{
				tasks.Add(productAttributeContainer.CreateItemAsync(attribute, new PartitionKey(attribute.PartitionKey))
						.ContinueWith(itemResponse =>
						{
							if (!itemResponse.IsCompletedSuccessfully)
							{
								AggregateException innerExceptions = itemResponse.Exception.Flatten();
								if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
								{
									Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
								}
								else
								{
									Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
								}
							}
						}));
			}
			await Task.WhenAll(tasks);
		}

		private async Task RemoveProductAttributes(Product product)
		{
			var productAttributeContainer = await LoadContainer<ProductAttribute>();
			var productAttributes = productAttributeContainer.GetItemLinqQueryable<ProductAttribute>(true).Where(b => b.ProductId == product.Id).ToList();

			var tasks = new List<Task>();
			foreach (var attribute in productAttributes)
			{
				tasks.Add(productAttributeContainer.DeleteItemAsync<ProductAttribute>(attribute.Id, new PartitionKey(attribute.PartitionKey))
						.ContinueWith(itemResponse =>
						{
							if (!itemResponse.IsCompletedSuccessfully)
							{
								AggregateException innerExceptions = itemResponse.Exception.Flatten();
								if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
								{
									Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
								}
								else
								{
									Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
								}
							}
						}));
			}
			await Task.WhenAll(tasks);
		}
	}
}
