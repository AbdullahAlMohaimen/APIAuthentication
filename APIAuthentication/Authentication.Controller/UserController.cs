using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Reflection;
using APIAuthentication.BO;
using APIAuthentication.Service;
using APIAuthentication.Global;

namespace APIAuthentication.Controller
{
	[Route("api/Users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		#region Users Endpoint

		#region GetUsersByID

		#endregion
		[HttpGet("GetUser/{ID}")]
		public IActionResult GetUser(int ID)
		{
			DataTable usersDataTable = new DataTable();
			string json;
			try
			{
				object[] parameters = new object[] { ID };
				MethodInvocation oMethodInvocation = new MethodInvocation("GetUser", EnumServiceInstance.UserService, parameters);
				usersDataTable = (DataTable)new UserService().DataWithReflection(oMethodInvocation);
				json = JsonConvert.SerializeObject(usersDataTable);
			}
			catch (Exception ex)
			{
				return StatusCode(500 , new { message = ex.Message});
			}
			return Content(json, "application/json");
		}
		#endregion
	}
}
