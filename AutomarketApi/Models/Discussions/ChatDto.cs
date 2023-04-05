using AutomarketApi.Models.Identity;
using AutomarketApi.Models.Car;

namespace AutomarketApi.Models.Discussions
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public List<User> Users { get; set; } = new();
        public Car.Car CarAbout { get; set; } = new();
        public bool IsDeleted { get; set; } = false;
        public List<MessageDto> Messages { get; set; } = new();
    }
}
