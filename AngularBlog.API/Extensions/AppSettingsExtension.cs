namespace BlazorBlog.API.Extensions
{
    public static class AppSettingsExtension
    {
        public static string GetConnectionString(string conString)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Directory.GetCurrentDirectory());
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString(conString);
        }

        public static string GetSectionValue(string sectionName)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Directory.GetCurrentDirectory());
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetSection(sectionName).Value;
        }
    }
}
