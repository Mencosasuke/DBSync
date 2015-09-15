using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBSync.Models
{
    public class Query
    {
        /// <summary>
        /// Flag que indica si la sentencia ya se ejecutó o no
        /// </summary>
        public String Pendiente { get; set; }

        /// <summary>
        /// Hora en la que fue ejecutada la sentencia
        /// </summary>
        public String Hora { get; set; }

        /// <summary>
        /// Base de datos en la que se debe ejecutar la sentencia
        /// </summary>
        public String TargetDatabase { get; set; }

        /// <summary>
        /// Indica que tipo de sentencia se ejecutó (Insert, Modify o Delete)
        /// </summary>
        public String TipoQuery { get; set; }

        /// <summary>
        /// Query que se va a ejecutar en caso el registro ya exista en la base de datos objetivo
        /// </summary>
        public String QueryString { get; set; }

        /// <summary>
        /// Query que se va a ejecutar en caso que el registro no exista en la base de datos objetivo
        /// </summary>
        public String QueryAlterno { get; set; }

        /// <summary>
        /// DPI del registro
        /// </summary>
        public String DpiOriginal { get; set; }

        /// <summary>
        /// DPI del registro si este fue cambiado
        /// </summary>
        public String DpiModificado { get; set; }

    }
}