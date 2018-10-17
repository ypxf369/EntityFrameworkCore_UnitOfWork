using Microsoft.EntityFrameworkCore;

namespace Host.Models
{
    public class CustomBlogRepository : Repository<Blog>
    {
        public CustomBlogRepository(BloggingContext dbContext) : base(dbContext)
        {

        }
    }
}
