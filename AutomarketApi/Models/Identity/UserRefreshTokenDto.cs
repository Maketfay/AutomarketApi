using System.ComponentModel.DataAnnotations.Schema;

namespace AutomarketApi.Models.Identity
{
    public class UserRefreshTokenDto: BaseEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
