using AutomarketApi.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutomarketApi.Models.Discussions
{
    public class Chat : BaseEntity
    {
        public virtual List<User>? Users { get; set; }

        [Required]
        public Guid CarAboutId { get; set; }
        [ForeignKey("CarAboutId")]
        public virtual Car.Car CarAbout { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<Message>? Messages { get; set; }
    }
}
