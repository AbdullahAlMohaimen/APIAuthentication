namespace APIAuthentication
{
	public class MethodInvocation
	{
		public MethodInvocation(string methodName, object serviceInstance, params object[] parameters)
		{
			MethodName = methodName;
			ServiceInstance = serviceInstance;
			Parameters = parameters;
		}
		public string MethodName { get; set; }
		public object ServiceInstance { get; set; }
		public object[] Parameters { get; set; }
	}
}
