using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;
using DBSync.Connection;

namespace DBSync.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            MySQLConnection conexionMySql = new MySQLConnection();
            //if (conexionMySql.insertarContacto() > 0)
            //{
            //    Debug.WriteLine("Al parecer si hizo el insert :)");
            //}
            
            return View();
        }

    }
}
