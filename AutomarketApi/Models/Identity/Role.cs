namespace AutomarketApi.Models.Identity
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
