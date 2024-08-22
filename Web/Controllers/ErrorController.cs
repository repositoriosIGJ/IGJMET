using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.Status = "404 Not Found";
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
