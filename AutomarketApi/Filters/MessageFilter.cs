using AutomarketApi.Models.Discussions;

namespace AutomarketApi.Filters
{
    public class MessageFilter:BaseFilter<Message>
    {
        public Guid? UserId { get; set; }
    }
}
