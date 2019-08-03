using System;

namespace ChatApp.Sql.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsExists { get; set; }
    }
}
