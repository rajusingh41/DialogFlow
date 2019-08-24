using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DialogFlowUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        /// <summary>
        /// https://docs.kommunicate.io/docs/web-installation
        /// </summary>
        /// <returns></returns>
       
       public ActionResult Other()
        {
            return View();

        }
    }
}
