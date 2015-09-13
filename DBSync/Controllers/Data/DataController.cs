using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBSync.Controllers.Home
{
    public class DataController : Controller
    {
        //
        // GET: /Data/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mysqlUpdateDelete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult pgsqlUpdateDelete()
        {
            return View();
        }

    }
}
