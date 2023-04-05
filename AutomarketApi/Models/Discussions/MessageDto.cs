using AutomarketApi.Models.Identity;

namespace AutomarketApi.Models.Discussions
{
    public class MessageDto
    {
        public User Sender { get; set; }
        public string MessageText { get; set; }
        public DateTime SentDate { get; set; }
        public ChatDto Chat { get; set; }
    }
}
