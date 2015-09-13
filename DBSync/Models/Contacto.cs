using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBSync.Models
{
    public class Contacto
    {

        /// <summary>
        /// Numero de DPI
        /// </summary>
        public String dpi { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        public String nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        public String apellido { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        public String direccion { get; set; }

        /// <summary>
        /// Telefono de Casa
        /// </summary>
        public String telefonoCasa { get; set; }

        /// <summary>
        /// Telefono de Movil
        /// </summary>
        public String telefonoMovil { get; set; }

        /// <summary>
        /// Nombre del Contacto
        /// </summary>
        public String nombreContacto { get; set; }

        /// <summary>
        /// Numero de Teléfono del Contacto
        /// </summary>
        public String numeroContacto { get; set; }

    }
}