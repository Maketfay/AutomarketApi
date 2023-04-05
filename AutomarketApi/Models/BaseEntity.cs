using System.ComponentModel.DataAnnotations;

namespace AutomarketApi.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } 
    }
}
