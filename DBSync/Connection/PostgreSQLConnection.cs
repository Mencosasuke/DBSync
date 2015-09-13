using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;
using System.Configuration;
using Npgsql;

namespace DBSync.Connection
{
    public class PostgreSQLConnection
    {

        /// <summary>
        /// Variable de conexión a base de datos de PostgreSQL
        /// </summary>
        private NpgsqlConnection conexion;

        /// <summary>
        /// Constructor de la clase conexión
        /// </summary>
        public PostgreSQLConnection()
        {
            conexion = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
        }

        /// <summary>
        /// Metodo para abrir la conexión a la base de datos MySQL
        /// </summary>
        /// <returns>Valor booleano para indicar si la conexión fue exitosa o no</returns>
        private bool open()
        {
            try
            {
                conexion.Open();
                Debug.WriteLine("Conexión a la base de datos exitosa");
                return true;
            }
            catch (NpgsqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Metodo para cerrar la conexion a la base de datos MySQL
        /// </summary>
        /// <returns>Valor booleando par aindicar si la conexion fue exitosa o no</returns>
        private bool close()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (NpgsqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Metodo para hacer un insert a la DB
        /// </summary>
        /// <returns></returns>
        public int insertarContacto()
        {
            int rowsAffected = 0;

            String query = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            query = String.Format(query, "2567648320101", "David", "Mencos", "Mi Casa", "numero1", "numero2", "Contacto 1", "numero 3");

            // Abre la conexión
            if (this.open())
            {
                // Crea la sentencia de ejecución del query
                NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();
                Debug.WriteLine("Sentencia ejecutada exitosamente");

                // Cierra la conexión
                this.close();
            }

            return rowsAffected;
        }

    }
}