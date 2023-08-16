using Clean.Blog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clean.Blog.Domain
{
    public class Comment : BaseDomainEntity
    {
        public int PostId { get; set; }
        public string Text { get; set; } = "";

        [JsonIgnore]
        public virtual Post? Post { get; set; }
    }
}
