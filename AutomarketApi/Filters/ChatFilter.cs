using AutomarketApi.Models.Discussions;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Filters
{
    public class ChatFilter:BaseFilter<Chat>
    {
        public Guid? UserId { get; set; }
        public override IQueryable<Chat> EnrichQuery(IQueryable<Chat> query)
        {
            query = query.Where(q => !q.IsDeleted);
            List<Chat> resQuery = new List<Chat> { };
            if (UserId.HasValue)
            {
                //query = query.Where(q=> q.Users.Where(d => d.Id == UserId.Value));
                
                foreach (var q in query)
                {
                    var res = q.Users.FirstOrDefault(u => u.Id == UserId.Value);
                    if (res != null) 
                    {
                        resQuery.Add(q);
                    }
                }
            }
            return resQuery.AsQueryable();
        }
    }
}
