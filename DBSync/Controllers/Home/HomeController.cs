﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DBSync.Connection;
//using System.Diagnostics;

namespace DBSync.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //MySQLConnection conexionMySql = new MySQLConnection();
            //PostgreSQLConnection conexionPostgreSQL = new PostgreSQLConnection();

            //if (conexionPostgreSQL.insertarContacto() > 0)
            //{
            //    Debug.WriteLine("Al parecer si hizo el insert :)");
            //}
            //if (conexionMySql.insertarContacto() > 0)
            //{
            //    Debug.WriteLine("Al parecer si hizo el insert :)");
            //}

            //if (conexionPostgreSQL.eliminarContacto("2567648320101") > 0)
            //{
            //    Debug.WriteLine("Al parecer si borro la tupla :)");
            //}
            //if (conexionMySql.eliminarContacto("2567648320101") > 0)
            //{
            //    Debug.WriteLine("Al parecer si borro la tupla :)");
            //}
            
            return View();
        }

    }
}
