using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tredi.API.DataServices;
using Tredi.API.Models;

namespace Tredi.API.Controllers
{
	[ApiController]
	[Produces("application/json")]
	[Route("[controller]")]
	public class PimController : ControllerBase
	{
		DataServiceCollection dataServices;

		public PimController(DataServiceCollection dataServices)
		{
			this.dataServices = dataServices;
		}
		
		[Authorize]
		[HttpGet("LoadAttributes")]
		public async Task<ActionResult> LoadAttributes()
		{
			var attributeList = await dataServices.PimService.LoadAttributes();
			return Ok(attributeList);
		}

		[Authorize]
		[HttpPost("AddAttribute")]
		public async Task<ActionResult> AddAttribute([FromBody] AttributeDto attribute)
		{
			var result = await dataServices.PimService.AddAttribute(attribute);
			return Ok(result);
		}

		[Authorize]
		[HttpPost("EditAttribute")]
		public async Task<ActionResult> EditAttribute([FromBody] AttributeDto attribute)
		{
			var result = await dataServices.PimService.EditAttribute(attribute);
			return Ok(result);
		}

		[Authorize]
		[HttpPost("DeleteAttribute")]
		public async Task<ActionResult> DeleteAttribute([FromBody] AttributeDto attribute)
		{
			var result = await dataServices.PimService.DeleteAttribute(attribute);
			return Ok(result);
		}


	}
}
