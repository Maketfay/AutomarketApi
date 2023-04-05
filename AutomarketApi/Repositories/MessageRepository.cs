using AutomarketApi.Context;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Repositories.Interfaces;

namespace AutomarketApi.Repositories
{
    public class MessageRepository:BaseRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
