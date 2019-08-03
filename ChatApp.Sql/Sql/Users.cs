using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ChatApp.Sql;

namespace ChatApp.Sql.Sql
{
    public class Users
    {
        public void AddUser(Models.Users user)
        {
            var repo = new Repository();
            using (var connection = repo.OpenConnection())
            {
                var addUserSql = @"INSERT INTO USERS (Name, PhoneNumber, EmailId, CreatedDate) VALUES (@Name, @PhoneNumber, @EmailId, @CreatedDate)";
                connection.Execute(addUserSql, user);
            }
        }
    }
}
