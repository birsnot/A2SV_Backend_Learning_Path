using Clean.Blog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Blog.Domain
{
    public class Post : BaseDomainEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";

        public ICollection<Comment> Comments { get; set; }
    }
}
