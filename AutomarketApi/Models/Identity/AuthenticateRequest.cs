using System.ComponentModel.DataAnnotations;

namespace AutomarketApi.Models.Identity
{
    public class AuthenticateRequest//for service 
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
