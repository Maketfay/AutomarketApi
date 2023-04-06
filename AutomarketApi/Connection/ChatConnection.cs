using AutomarketApi.Models.Discussions;

namespace AutomarketApi.Connection
{
    public class ChatConnection
    {
        public UserChatViewModel User { get; set; }
        public Guid ChatId { get; set; }
    }
}
