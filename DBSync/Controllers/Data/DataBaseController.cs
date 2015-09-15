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
        /// <param name="modContacto">Contacto a modificar (opcional)</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult mysqlUpdateDelete(Contacto modContacto)
        {
            DataHelper dataHelper = new DataHelper();

            if (modContacto.dpi != null)
            {
                ViewBag.ContactoModificar = modContacto;
            }

            List<Contacto> listaContactos = new List<Contacto>();

            // Arma la lista de los contactos obtenidos en la base de datos de MySQL
            listaContactos = dataHelper.ArmarListaContactosMySQL(conexionMySQL.ObtenerRegistros()).OrderBy(lc => lc.dpi).ThenBy(lc => lc.nombre).ThenBy(lc => lc.apellido).ToList();

            ViewBag.ListaContactos = listaContactos;

            return View();
        }

        /// <summary>
        /// Renderiza la vista parcial para el mantenimiento de contactos PostgreSQL
        /// </summary>
        /// <param name="modContacto">Contacto a modificar (opcional)</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult pgsqlUpdateDelete(Contacto modContacto)
        {
            DataHelper dataHelper = new DataHelper();

            if (modContacto.dpi != null)
            {
                ViewBag.ContactoModificar = modContacto;
            }

            List<Contacto> listaContactos = new List<Contacto>();

            // Arma la lista de los contactos obtenidos en la base de datos de PostgreSQL
            listaContactos = dataHelper.ArmarListaContactosPgSQL(conexionPgSQL.ObtenerRegistros()).OrderBy(lc => lc.dpi).ThenBy(lc => lc.nombre).ThenBy(lc => lc.apellido).ToList();

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
            newContacto.nombre = txtNombre;
            newContacto.apellido = txtApellido;
            newContacto.direccion = txtDireccion;
            newContacto.telefonoCasa = txtTelefonoCasa;
            newContacto.telefonoMovil = txtTelefonoMovil;
            newContacto.nombreContacto = txtNombrecontacto;
            newContacto.numeroContacto = txtTelefonoContacto;

            int rowsAffected = conexionMySQL.InsertarContacto(newContacto);

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
            newContacto.nombre = txtNombre;
            newContacto.apellido = txtApellido;
            newContacto.direccion = txtDireccion;
            newContacto.telefonoCasa = txtTelefonoCasa;
            newContacto.telefonoMovil = txtTelefonoMovil;
            newContacto.nombreContacto = txtNombrecontacto;
            newContacto.numeroContacto = txtTelefonoContacto;

            int rowsAffected = conexionPgSQL.InsertarContacto(newContacto);

            return RedirectToAction("Index", "Home", new { load = "pgsql" });
        }

        /// <summary>
        /// Modifica un contacto en la base de datos MySQL
        /// </summary>
        /// <param name="dpiOriginal">DPI original</param>
        /// <param name="txtDpi2">DPI</param>
        /// <param name="txtNombre2">Nombre</param>
        /// <param name="txtApellido2">Apellido</param>
        /// <param name="txtDireccion2">Dirección</param>
        /// <param name="txtTelefonoCasa2">Teléfono de Casa</param>
        /// <param name="txtTelefonoMovil2">Teléfono de Movil</param>
        /// <param name="txtNombrecontacto2">Nombre de Contacto</param>
        /// <param name="txtTelefonoContacto2">Teléfono de Contacto</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModificarContactoMySQL(String dpiOriginal, String txtDpi2, String txtNombre2, String txtApellido2, String txtDireccion2, String txtTelefonoCasa2, String txtTelefonoMovil2, String txtNombrecontacto2, String txtTelefonoContacto2)
        {
            Contacto contacto = new Contacto();

            contacto.dpi = txtDpi2;
            contacto.nombre = txtNombre2;
            contacto.apellido = txtApellido2;
            contacto.direccion = txtDireccion2;
            contacto.telefonoCasa = txtTelefonoCasa2;
            contacto.telefonoMovil = txtTelefonoMovil2;
            contacto.nombreContacto = txtNombrecontacto2;
            contacto.numeroContacto = txtTelefonoContacto2;

            int rowsAffected = conexionMySQL.ModificarContacto(dpiOriginal, contacto);

            return RedirectToAction("Index", "Home", new { load = "mysql" });
        }

        /// <summary>
        /// Modifica un contacto en la base de datos PostgreSQL
        /// </summary>
        /// <param name="dpiOriginal">DPI original</param>
        /// <param name="txtDpi2">DPI</param>
        /// <param name="txtNombre2">Nombre</param>
        /// <param name="txtApellido2">Apellido</param>
        /// <param name="txtDireccion2">Dirección</param>
        /// <param name="txtTelefonoCasa2">Teléfono de Casa</param>
        /// <param name="txtTelefonoMovil2">Teléfono de Movil</param>
        /// <param name="txtNombrecontacto2">Nombre de Contacto</param>
        /// <param name="txtTelefonoContacto2">Teléfono de Contacto</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModificarContactoPgSQL(String dpiOriginal, String txtDpi2, String txtNombre2, String txtApellido2, String txtDireccion2, String txtTelefonoCasa2, String txtTelefonoMovil2, String txtNombrecontacto2, String txtTelefonoContacto2)
        {
            Contacto contacto = new Contacto();

            contacto.dpi = txtDpi2;
            contacto.nombre = txtNombre2;
            contacto.apellido = txtApellido2;
            contacto.direccion = txtDireccion2;
            contacto.telefonoCasa = txtTelefonoCasa2;
            contacto.telefonoMovil = txtTelefonoMovil2;
            contacto.nombreContacto = txtNombrecontacto2;
            contacto.numeroContacto = txtTelefonoContacto2;

            int rowsAffected = conexionPgSQL.ModificarContacto(dpiOriginal, contacto);

            return RedirectToAction("Index", "Home", new { load = "pgsql" });
        }

    }
}
