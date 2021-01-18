using BoilerPlateDemo_App.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlateDemo_App.Web.Host.Controllers
{
    public class HomeController : BoilerPlateDemo_AppControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment)
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
