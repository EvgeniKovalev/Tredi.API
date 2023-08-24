using Microsoft.Azure.Cosmos;
using Tredi.API.Models;

namespace Tredi.API.DataServices
{
	public class UserService
	{
		private const int _cryptFactor = 13;
		private CosmosClient _client;

		public UserService(CosmosClient cosmosClient)
		{
			_client = cosmosClient;
		}

		private async Task<Container> LoadContainer()
		{
			Database database = await _client.CreateDatabaseIfNotExistsAsync("TrediDB");
			return await database.CreateContainerIfNotExistsAsync("User", "/PartitionKey");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userDto"></param>
		/// <returns></returns>
		public async Task AddUser(UserDto userDto)
		{
			//var container = await LoadContainer();
			//try
			//{
			//	var existingUserResponse = await container.ReadItemAsync<User>(userDto.Id, new PartitionKey(userDto.PartitionKey));
			//}
			//catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			//{
			//	var existingUserResponse = await container.CreateItemAsync(userDto, new PartitionKey(userDto.PartitionKey));
			//	Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", existingUserResponse.Resource.Id, existingUserResponse.RequestCharge);
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<UserDto> GetUserByLogin(LoginDto form)
		{
			var container = await LoadContainer();

			var user = container.GetItemLinqQueryable<DataModels.User>(true).Where(b => b.UserName == form.Username).AsEnumerable().FirstOrDefault();
			if (user == null)
			{
				throw new Exception("USER_NOT_FOUND");
			}

			if (user != null && user.IsActive == false)
			{
				throw new Exception("USER_DEACTIVATED");
			}

			if (user != null && !BCrypt.Net.BCrypt.EnhancedVerify(form.Password, user.Password))
			{
				throw new Exception("PASSWORD_MISMATCH");
			}

			return new UserDto()
			{
				Id = user.Id,
				UserName = user.UserName,
				Name = user.Name
			};
		}
	}
}
