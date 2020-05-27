using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalBlog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var blogPost = context.BlogPosts.SingleOrDefault(post => post.Id == id);
            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> AddBlogPost()
        {
            BlogPost post = new BlogPost();
            var user = await userManager.GetUserAsync(User);
            post.UserId = user?.Id;
            return View(post);
        }

        [HttpPost]
        public IActionResult AddBlogPost(BlogPost blogPost)
        {
            context.BlogPosts.Add(blogPost);
            context.SaveChanges();
            return RedirectToAction("Details",new { id = blogPost.Id});
        }


        public IActionResult DeleteBlogPost()
        {
            return View();
        }
        
    }
}
