using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlateDemo_App.Web.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            //In production we have combined API and SPA projects so it can be directly served by index.html
            if (_hostingEnvironment.IsProduction())
            {
                return Redirect("/index.html");
            }
            //In Local we run SPA separately from ng serve, so API project by default serves Swagger API page
            else
            {
                return Redirect("/swagger");
            }
        }
    }
}
