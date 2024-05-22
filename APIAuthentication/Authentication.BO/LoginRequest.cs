using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAuthentication.BO
{
	public class LoginRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string SecurityKey { get; set; }
	}
}
