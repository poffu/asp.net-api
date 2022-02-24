using System.Text.Json.Serialization;

namespace WebAppAPI.Dto
{
    public record UserDto
    {
        public int UserId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Tel { get; init; }
        public string Password { get; init; }
        [JsonIgnore]
        public int Rule { get; init; }

        public UserDto()
        {
            UserId = 0;
            Name = string.Empty;
            Email = string.Empty;
            Tel = string.Empty;
            Password = string.Empty;
            Rule = 1;
        }
    }
}