using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PersonalBlog.Data;
using PersonalBlog.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalBlog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public AdminController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
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
            BlogPostViewModel post = new BlogPostViewModel();
            var user = await userManager.GetUserAsync(User);
            post.UserId = user?.Id;
            return View(post);
        }

        [HttpPost]
        public IActionResult AddBlogPost(BlogPostViewModel model)
        {
            BlogPost blogPost = new BlogPost();
            if (ModelState.IsValid)
            {
                var uniquefilename = UploadedFile(model);


                blogPost.Image = uniquefilename;
                blogPost.Summary = model.Summary;
                blogPost.Body = model.Body;
                blogPost.Title = model.Title;
                blogPost.UserId = model.UserId;
               

                context.BlogPosts.Add(blogPost);
                context.SaveChanges();
            }

            return RedirectToAction("Details",new { id = blogPost.Id});
        }


        public IActionResult DeleteBlogPost()
        {
            return View();
        }

        private string UploadedFile(BlogPostViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
