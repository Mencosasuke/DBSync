using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DBSync.Models;
using DBSync.Connection;
using DBSync.Helper;

namespace DBSync.Controllers.Home
{
    public class DataBaseController : Controller
    {

        /// <summary>
        /// Instancia a la conexión MySQL
        /// </summary>
        private MySQLConnection conexionMySQL = new MySQLConnection();

        /// <summary>
        /// Instancia a la conexión PostgreSQL
        /// </summary>
        private PostgreSQLConnection conexionPgSQL = new PostgreSQLConnection();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renderiza la vista parcial para el mantenimiento de contactos MySQL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult mysqlUpdateDelete()
        {
            DataHelper dataHelper = new DataHelper();
            List<Contacto> listaContactos = new List<Contacto>();

            // Arma la lista de los contactos obtenidos en la base de datos de MySQL
            listaContactos = dataHelper.ArmarListaContactosMySQL(conexionMySQL.ObtenerRegistros());
            
            ViewBag.ListaContactos = listaContactos;

            return View();
        }

        /// <summary>
        /// Renderiza la vista parcial para el mantenimiento de contactos PostgreSQL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult pgsqlUpdateDelete()
        {
            DataHelper dataHelper = new DataHelper();
            List<Contacto> listaContactos = new List<Contacto>();

            // Arma la lista de los contactos obtenidos en la base de datos de PostgreSQL
            listaContactos = dataHelper.ArmarListaContactosPgSQL(conexionPgSQL.ObtenerRegistros());

            ViewBag.ListaContactos = listaContactos;

            return View();
        }

        /// <summary>
        /// Inserta un nuevo contacto a la base de datos MySQL
        /// </summary>
        /// <param name="txtDpi">Número de DPI</param>
        /// <param name="txtNombre">Nombre</param>
        /// <param name="txtApellido">Apellido</param>
        /// <param name="txtDireccion">Dirección</param>
        /// <param name="txtTelefonoCasa">Teléfono de Casa</param>
        /// <param name="txtTelefonoMovil">Teléfono de Movil</param>
        /// <param name="txtNombrecontacto">Nombre de Contacto</param>
        /// <param name="txtTelefonoContacto">Teléfono de Contacto</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertarContactoMySQL(String txtDpi, String txtNombre, String txtApellido, String txtDireccion, String txtTelefonoCasa, String txtTelefonoMovil, String txtNombrecontacto, String txtTelefonoContacto)
        {
            Contacto newContacto = new Contacto();

            newContacto.dpi = txtDpi;
            newContacto.nombreContacto = txtNombre;
            newContacto.apellido = txtApellido;
            newContacto.direccion = txtDireccion;
            newContacto.telefonoCasa = txtTelefonoCasa;
            newContacto.telefonoMovil = txtTelefonoMovil;
            newContacto.nombreContacto = txtNombrecontacto;
            newContacto.numeroContacto = txtTelefonoContacto;

            conexionMySQL.InsertarContacto(newContacto);

            return RedirectToAction("Index", "Home", new { load = "mysql" });
        }

        /// <summary>
        /// Inserta un nuevo contacto a la base de datos PostgreSQL
        /// </summary>
        /// <param name="txtDpi">Número de DPI</param>
        /// <param name="txtNombre">Nombre</param>
        /// <param name="txtApellido">Apellido</param>
        /// <param name="txtDireccion">Dirección</param>
        /// <param name="txtTelefonoCasa">Teléfono de Casa</param>
        /// <param name="txtTelefonoMovil">Teléfono de Movil</param>
        /// <param name="txtNombrecontacto">Nombre de Contacto</param>
        /// <param name="txtTelefonoContacto">Teléfono de Contacto</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertarContactoPgSQL(String txtDpi, String txtNombre, String txtApellido, String txtDireccion, String txtTelefonoCasa, String txtTelefonoMovil, String txtNombrecontacto, String txtTelefonoContacto)
        {
            Contacto newContacto = new Contacto();

            newContacto.dpi = txtDpi;
            newContacto.nombreContacto = txtNombre;
            newContacto.apellido = txtApellido;
            newContacto.direccion = txtDireccion;
            newContacto.telefonoCasa = txtTelefonoCasa;
            newContacto.telefonoMovil = txtTelefonoMovil;
            newContacto.nombreContacto = txtNombrecontacto;
            newContacto.numeroContacto = txtTelefonoContacto;

            conexionPgSQL.InsertarContacto(newContacto);

            return RedirectToAction("Index", "Home", new { load = "pgsql" });
        }

    }
}
