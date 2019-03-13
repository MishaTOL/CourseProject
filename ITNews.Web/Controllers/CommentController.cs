using Microsoft.AspNetCore.Mvc;

namespace ITNews.Web.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}