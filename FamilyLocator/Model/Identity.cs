using System;
using SQLite;

namespace FamilyLocator.Model
{
    [Table("Identity")]
    public class Identity
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string EmailId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public string PhoneModel { get; set; }

        public bool  IsFriend { get; set; }

        public Identity()
        {

        }
    }
}