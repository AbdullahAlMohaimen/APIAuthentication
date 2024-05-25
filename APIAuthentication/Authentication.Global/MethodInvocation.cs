using APIAuthentication.Authentication.Service;
using APIAuthentication.Global;
using APIAuthentication.Service;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace APIAuthentication
{
	#region Method Invocation
	public class MethodInvocation
	{
		#region Constructor
		public MethodInvocation(string methodName, EnumServiceInstance instance, params object[] parameters)
		{
			MethodName = methodName;
			ServiceInstance = CreateServiceInstances(instance);
			Parameters = parameters;
			ParameterTypes = parameters != null ? Array.ConvertAll(parameters, param => param.GetType()) : Type.EmptyTypes;
		}

		#endregion

		#region Property
		public string MethodName { get; set; }
		public object ServiceInstance { get; set; }
		public object[] Parameters { get; set; }
		public Type[] ParameterTypes { get; set; }
		#endregion

		#region ServiceInstance Creation
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
		#endregion
	}
	#endregion

	#region Calling Method Information
	public class CallingMethodInformation
	{
		public CallingMethodInformation(string stackTrace)
		{
			ParameterDataTypes = new List<string>();
			string[] stackLines = stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			string callerMethod = stackLines.Length > 3 ? stackLines[3] : "Unknown";
			ParseMethodLine(callerMethod);
		}

		public string FunctionName { get; set; }
		public int NoOfParameters { get; set; }
		public List<string> ParameterDataTypes { get; set; }

		private void ParseMethodLine(string methodLine)
		{
			Regex methodRegex = new Regex(@"\s+at\s+(?<method>.+)\((?<params>.*)\)");
			Match match = methodRegex.Match(methodLine);

			if (match.Success)
			{
				string fullMethodName = match.Groups["method"].Value;
				string parameters = match.Groups["params"].Value;
				string functionName = fullMethodName.Contains(".") ? fullMethodName.Substring(fullMethodName.LastIndexOf('.') + 1) : fullMethodName;
				FunctionName = functionName;
				if (!string.IsNullOrEmpty(parameters))
				{
					string[] paramList = parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					NoOfParameters = paramList.Length;
					foreach (string param in paramList)
					{
						string paramType = ExtractParameterType(param);
						ParameterDataTypes.Add(paramType);
					}
				}
				else
				{
					NoOfParameters = 0;
				}
			}
			else
			{
				FunctionName = "Unknown";
				NoOfParameters = 0;
			}
		}

		private string ExtractParameterType(string param)
		{
			string[] parts = param.Trim().Split(' ');
			if (parts.Length > 1)
			{
				return parts[0];
			}
			return "Unknown";
		}
	}
	#endregion
}
