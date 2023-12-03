using System.Data.SqlClient;

namespace IT_Hardware
{
    public class DBConnection
    {
        public SqlConnection con { get; set; }

        public DBConnection()
        {
            var configuation = GetConfiguration();
            con = new SqlConnection(configuation.GetSection("ConnectionStrings").GetSection("SQLConnection").Value);
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
