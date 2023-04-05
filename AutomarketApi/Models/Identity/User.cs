using AutomarketApi.Models.Discussions;
using AutomarketApi.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutomarketApi.Models.Identity
{
    public class User : BaseEntity//for db
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
        public string Password { get; set; }
        
        public virtual List<Chat>? Chats { get; set; }
    }
}
