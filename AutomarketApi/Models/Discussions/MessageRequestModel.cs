namespace AutomarketApi.Models.Discussions
{
    public class MessageRequestModel
    {
        public Guid ChatId { get; set; }
        public string SenderUsername { get; set; }
        public string Message { get; set; }
        
    }
}
