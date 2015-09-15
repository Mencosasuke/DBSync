using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DBSync.Connection;
using System.Web.Security;

namespace DBSync.Controllers.Home
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(String usuario, String password)
        {
            PostgreSQLConnection conexionMySQL = new PostgreSQLConnection();
            MySQLConnection conexionPgSLQ = new MySQLConnection();

            if (conexionPgSLQ.Login(usuario, password) || conexionMySQL.Login(usuario, password))
            {
                Session["usuario"] = usuario;
                return RedirectToAction("Index", "Home", new { load = "mysql" });
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session["usuario"] = null;

            return RedirectToAction("Login", "Login");
        }

    }
}
