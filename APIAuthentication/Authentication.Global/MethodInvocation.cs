using APIAuthentication.Authentication.Service;
using APIAuthentication.Global;
using APIAuthentication.Service;

namespace APIAuthentication
{
	public class MethodInvocation
	{
		public MethodInvocation(string methodName, EnumServiceInstance instance, params object[] parameters)
		{
			MethodName = methodName;
			ServiceInstance = CreateServiceInstances(instance);
			Parameters = parameters;
		}
		public string MethodName { get; set; }
		public object ServiceInstance { get; set; }
		public object[] Parameters { get; set; }

		private object CreateServiceInstances(EnumServiceInstance instance)
		{
			object ServiceInstance = null;
			switch (instance)
			{
				case EnumServiceInstance.AuthenticationService :
					AuthenticationService authenticationService = new AuthenticationService();
					ServiceInstance = authenticationService;
					break;
				case EnumServiceInstance.RoleService:
					RoleService roleService = new RoleService();
					ServiceInstance = roleService;
					break;
				case EnumServiceInstance.UserService:
					UserService userService = new UserService();
					ServiceInstance = userService;
					break;
				case EnumServiceInstance.EmployeeService:
					EmployeeService employeeService = new EmployeeService();
					ServiceInstance = employeeService;
					break;
				case EnumServiceInstance.LoginInfoService:
					LoginInfoService loginInfoService = new LoginInfoService();
					ServiceInstance = loginInfoService;
					break;
				case EnumServiceInstance.UserPasswordHistoryService:
					UserPasswordHistoryService userPasswordHistoryService = new UserPasswordHistoryService();
					ServiceInstance = userPasswordHistoryService;
					break;
				case EnumServiceInstance.EmployeePasswordHistoryService:
					EmployeePasswordHistoryService employeePasswordHistoryService = new EmployeePasswordHistoryService();
					ServiceInstance = employeePasswordHistoryService;
					break;
				case EnumServiceInstance.PasswordResetHistoryService:
					PasswordResetHistoryService passwordResetHistoryService = new PasswordResetHistoryService();
					ServiceInstance = passwordResetHistoryService;
					break;
				case EnumServiceInstance.BadLoginAttemptInfoService:
					BadLoginAttemptInfoService badLoginAttemptInfoService = new BadLoginAttemptInfoService();
					ServiceInstance = badLoginAttemptInfoService;
					break;
				default :
					throw new ArgumentException("Service not found");
					break;
			}

			return ServiceInstance;
		}
	}
}
