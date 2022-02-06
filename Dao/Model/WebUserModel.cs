using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAPI.Dao.Model
{
    [Table("web_user")]
    public record WebUserModel
    {
        [Column("user_id")]
        public int user_id { get; init; }
        [Column("name")]
        public string name { get; init; }
        [Column("email")]
        public string email { get; init; }
        [Column("tel")]
        public string tel { get; init; }
        [Column("password")]
        public string password { get; init; }
        [Column("rule")]
        public int rule { get; init; }
    }
}