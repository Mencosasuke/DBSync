using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DBSync.Connection
{
    public class MySQLConnection
    {
        /// <summary>
        /// Variable de conexión a base de datos de MySQL
        /// </summary>
        private MySqlConnection conexion;

        /// <summary>
        /// Variable de ejecución de comandos
        /// </summary>
        private MySqlCommand cmd;

        /// <summary>
        /// Variable donde se guarda el resultado de las consultas select
        /// </summary>
        private MySqlDataReader dataReader;

        /// <summary>
        /// Variable que guarda la sentencia a ejecutar
        /// </summary>
        private String query;

        /// <summary>
        /// Variable que indica cuantas tuplas fueron modificadas
        /// </summary>
        private int rowsAffected;

        /// <summary>
        /// Constructor de la clase conexión
        /// </summary>
        public MySQLConnection()
        {
            conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySql"].ConnectionString);
        }

        /// <summary>
        /// Metodo para abrir la conexión a la base de datos MySQL
        /// </summary>
        /// <returns>Valor booleano para indicar si la conexión fue exitosa o no</returns>
        private bool Open()
        {
            try
            {
                conexion.Open();
                Debug.WriteLine("Conexión a la base de datos exitosa");
                return true;
            }catch(MySqlException e)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (e.Number)
                {
                    case 0:
                        Debug.WriteLine("Imposible conectar al servidor.");
                        break;
                    case 1045:
                        Debug.WriteLine("Nombre de usuario o contraseña invalida.");
                        break;
                }

                return false;
            }
        }

        /// <summary>
        /// Metodo para cerrar la conexion a la base de datos MySQL
        /// </summary>
        /// <returns>Valor booleando par aindicar si la conexion fue exitosa o no</returns>
        private bool Close()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Metodo para hacer un insert a la base de datos MySQL
        /// </summary>
        /// <returns>Cantidad de tuplas afectadas</returns>
        public int InsertarContacto()
        {
            rowsAffected = 0;

            query = String.Empty;
            query = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            query = String.Format(query, "2567648320101", "David", "Mencos", "Mi Casa", "numero1", "numero2", "Contacto 1", "numero 3");

            // Abre conexión
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();
                Debug.WriteLine("Sentencia ejecutada exitosamente");

                // Cierra la conexión
                this.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Metodo para modificar un contacto de la base de datos MySQL
        /// </summary>
        /// <param name="dpi">Numero de DPI que identifique al contacto que se desea modificar</param>
        /// <returns>Cantidad de tuplas afectadas</returns>
        public int ModificarContacto(String dpi)
        {

            rowsAffected = 0;

            query = String.Empty;
            query = "UPDATE contacto SET dpi='{0}', nombre='{1}', apellido='{2}', direccion='{3}', telefono_casa='{4}', telefono_movil='{5}', nombre_contacto='{6}', numero_telefono_contacto='{7}' WHERE dpi='{0}'";
            //query = String.Format(query, dpi);

            //Open connection
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Cierrla la conexión
                this.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Metodo para eliminar un contacto de la base de datos MySQL
        /// </summary>
        /// <param name="dpi">Numero de DPI que identifique al contacto que se desea eliminar</param>
        /// <returns>Cantidad de tuplas afectadas</returns>
        public int EliminarContacto(String dpi)
        {

            rowsAffected = 0;

            query = String.Empty;
            query = String.Format("DELETE FROM contacto WHERE dpi='{0}'", dpi);

            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Cierra la conexión
                this.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Ejecuta la sentencia select para obtener todos los registros de la tabla contacto
        /// </summary>
        /// <returns>DataReader con todos los resultados obtenidos del select</returns>
        public MySqlDataReader ObtenerRegistros()
        {
            dataReader = null;

            query = String.Empty;
            query = "SELECT * FROM contacto";

            // Abre la conexion
            if (this.Open())
            {
                // Crea la sentencia de ejecucion del select
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia y la guarda en el dataReader
                dataReader = cmd.ExecuteReader();
            }

            return dataReader;
        }
    }
}