using System.Web.Mvc;

namespace Voyage.Api.Controllers
{
    public class HomeController : Controller
    {
        // Redirect the default landing page to the API helper page.
        [AllowAnonymous]
        public ActionResult Index()
        {
            return new RedirectResult("~/swagger/ui/index");
        }
    }
}