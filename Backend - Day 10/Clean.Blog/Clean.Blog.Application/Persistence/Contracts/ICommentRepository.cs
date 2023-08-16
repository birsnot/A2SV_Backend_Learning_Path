using Clean.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Blog.Application.Persistence.Contracts
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
    }
}
