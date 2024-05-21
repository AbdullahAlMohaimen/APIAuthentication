using Microsoft.Extensions.Configuration;

namespace APIAuthentication
{
	public static class ConfigurationManager
	{
		private static IConfiguration _configuration;
		private static IConfigurationSection _dbConfig;
		
		public static void Initialize(IConfiguration configuration)
		{
			_configuration = configuration;
			_dbConfig = _configuration.GetSection("dbSettings");
		}
		public static string ConnectionString => _dbConfig["DefaultConnection"];
	}
}
