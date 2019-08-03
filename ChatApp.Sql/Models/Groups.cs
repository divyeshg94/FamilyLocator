using System;

namespace ChatApp.Sql.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsExists { get; set; }
    }
}
