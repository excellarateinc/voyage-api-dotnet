using System.Web.Mvc;

namespace Voyage.Web.Controllers
{
    public class HomeController : Controller
    {
        // Redirect the default landing page to the API helper page
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectPermanent("~/docs");
        }
    }
}