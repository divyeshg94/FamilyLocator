using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ChatApp.Sql
{
    public class Repository
    {
        protected readonly IDbConnectionFactory connectionFactory;

        public IDbConnection OpenConnection()
        {
            //var dbSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            var dbSettings = "Data Source=localhost;Initial Catalog=chatApp; User Id=sa; password=pass@word1;";
            var connection = new SqlConnection(dbSettings);
            connection.Open();

            return connection;
        }
    }
}