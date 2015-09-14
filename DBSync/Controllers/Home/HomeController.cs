using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DBSync.Connection;

namespace DBSync.Controllers.Home
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Renderiza la vista principal de la aplicación
        /// </summary>
        /// <param name="load">Parametro opcinal, indica cual panel de DB se debe mostrar primero.</param>
        /// <returns></returns>
        public ActionResult Index(String load)
        {
            ViewBag.load = load;
            return View();
        }

    }
}
