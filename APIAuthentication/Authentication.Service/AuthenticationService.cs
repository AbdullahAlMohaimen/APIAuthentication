using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using APIAuthentication.BO;

namespace APIAuthentication.Service
{
	public class AuthenticationService
	{
		private readonly IConfiguration _configuration;
		public AuthenticationService()
		{

		}
		public AuthenticationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		#region Token Generate
		public string CreateToken(LoginRequest loginRequest)
		{
			var jwtConfig = _configuration.GetSection("Jwt");
			var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, jwtConfig["Subject"]),
						new Claim("UserName", loginRequest.UserName),
						new Claim("Password", loginRequest.Password),
					};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
			var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(jwtConfig["Issuer"], jwtConfig["Audience"],
				claims,
				expires: DateTime.UtcNow.AddMinutes(10),
				signingCredentials: signIn);

			var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
			return jwtToken;
		}
		#endregion
	}
}
