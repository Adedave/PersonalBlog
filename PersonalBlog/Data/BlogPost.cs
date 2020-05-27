using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.Data
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        [NotMapped]
        public string[] Tags { get; set; }
        public int AuthorId { get; set; }
        public string UserId { get; set; }
        public Author Author { get; set; }
    }
}
