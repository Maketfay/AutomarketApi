using AutomarketApi.Models.Identity;

namespace AutomarketApi.Models.Identity
{
    public class AuthenticateResponse//for service
    {
        public Guid UserId { get; set; }

        public TokenViewModel Token { get; set; } 
        
    }
}

