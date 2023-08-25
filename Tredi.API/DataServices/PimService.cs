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

		private async Task<Container> LoadContainer()
		{
			Database database = await _client.CreateDatabaseIfNotExistsAsync("TrediDB");
			return await database.CreateContainerIfNotExistsAsync("Attribute", "/PartitionKey");
		}

		public async Task<bool> AddAttribute(AttributeDto attributeDto)
		{
			var attribute = new ProductAttribute(attributeDto);
			var container = await LoadContainer();
			var createdResponse = await container.CreateItemAsync(attribute, new PartitionKey(attribute.PartitionKey));
			return createdResponse.StatusCode == System.Net.HttpStatusCode.OK;
		}

		public async Task<List<AttributeDto>> LoadAttributes()
		{
			var container = await LoadContainer();
			var attributes = container.GetItemLinqQueryable<ProductAttribute>(true).Where(b => b.IsActive).ToList();

			var dtoList = new List<AttributeDto>();
			if (attributes.Any())
			{
				attributes.ForEach(a => { dtoList.Add(new AttributeDto(a)); });
			}
			return dtoList;
		}
	}
}
