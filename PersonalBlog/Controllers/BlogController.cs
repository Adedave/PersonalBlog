using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext context;

        public BlogController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<BlogPost> blogPosts =  context.BlogPosts
                                                    .OrderByDescending(x => x.DateCreated)
                                                    .ToList();
            return View(blogPosts);
        }
    }
}
