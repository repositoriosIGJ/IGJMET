using System.Web.Mvc;
using System.Web.Security;
using Web.Filters;
using WebMatrix.WebData;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeGroup]
        public ActionResult Index()
        {
            return View();
        }
    }
}
