using AutomarketApi.Context;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Repositories.Interfaces;

namespace AutomarketApi.Repositories
{
    public class ChatRepository:BaseRepository<Chat>, IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context):base(context)
        {
        }
    }
}
