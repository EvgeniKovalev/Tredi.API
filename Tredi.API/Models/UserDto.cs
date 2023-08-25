using Tredi.API.DataServices.DataModels;

namespace Tredi.API.Models
{
	public class UserDto
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }

		public UserDto() { }
		public UserDto(User? user)
		{
			if (user != null)
			{
				Id = user.Id;
				UserName = user.UserName;
				Name = user.Name;
			}
		}
	}
}
