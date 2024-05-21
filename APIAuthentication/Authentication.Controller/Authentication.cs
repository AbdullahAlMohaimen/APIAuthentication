using System;
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
using Microsoft.Extensions.Configuration;
using APIAuthentication.BO;
using APIAuthentication.Service;

namespace APIAuthentication.Controller
{
	[Route("api/Authentication")]
	[ApiController]
	public class Authentication : ControllerBase
	{
		private readonly IConfiguration _config;
		public Authentication(IConfiguration config)
		{
			_config = config;
		}

		[HttpPost("Login")]
		public IActionResult Login([FromBody] LoginRequest loginModel)
		{
			LoginRequest oLoginRequest = new LoginRequest();
			LoginRequest oCurrentUser = new LoginRequest();
			string json = "";
			string userToken = "";
			try
			{
				var oUserCredentials = _config.GetSection("UserCredentials");
				oLoginRequest.UserName = oUserCredentials["Username"];
				oLoginRequest.Password = oUserCredentials["Password"];

				if (oLoginRequest.UserName == loginModel.UserName && oLoginRequest.Password == loginModel.Password)
				{
					oCurrentUser.UserName = oLoginRequest.UserName;
					oCurrentUser.Password = oLoginRequest.Password;

					userToken = new AuthenticationService(_config).CreateToken(oCurrentUser);
				}
				else
				{
					return Unauthorized(new { message = "Invalid username or password." });
				}
			}
			catch (Exception e)
			{
				return StatusCode(500, new { message = e.Message });
			}
			return Ok(userToken);
		}
	}
}
