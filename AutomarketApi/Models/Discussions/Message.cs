using AutomarketApi.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace AutomarketApi.Models.Discussions
{
    public class Message : BaseEntity
    {
        [Required]
        public virtual User Sender { get; set; }
        [Required]
        public string MessageText { get; set; }
        [Required]
        public DateTime SentDate { get; set; }
        [Required]
        public virtual Chat Chat { get; set; }
    }
    
}
