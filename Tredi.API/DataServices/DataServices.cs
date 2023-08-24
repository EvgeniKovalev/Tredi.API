using Microsoft.Azure.Cosmos;

namespace Tredi.API.DataServices
{
	public class DataServiceCollection
	{
		public UserService UserService { get; set; }

		public DataServiceCollection(CosmosClient cosmosClient)
		{
			UserService = new UserService(cosmosClient);
		}
	}
}
