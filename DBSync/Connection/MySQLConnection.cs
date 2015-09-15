using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DBSync.Models;
using DBSync.Helper;

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
        /// Variable que indica cuantas tuplas fueron modificadas
        /// </summary>
        private int rowsAffected;

        /// <summary>
        /// Instancia de la clase DataHelper
        /// </summary>
        private DataHelper dh = new DataHelper();

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
        public bool Close()
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
        public int InsertarContacto(Contacto contacto)
        {
            rowsAffected = 0;

            String query = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            query = String.Format(query, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto);
            
            // Ejecuta la sentencia si el dpi no existe en la base de datos
            if (!this.ConsultarRegistro(contacto.dpi))
            {
                // Abre conexión
                if (this.Open())
                {
                    // Crea la sentencia de ejecución del query
                    cmd = new MySqlCommand(query, conexion);
                    rowsAffected = cmd.ExecuteNonQuery();
                    Debug.WriteLine("Sentencia ejecutada exitosamente");

                    // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                    if (rowsAffected > 0)
                    {
                        String queryAlterno = "UPDATE contacto SET dpi='{0}', nombre='{1}', apellido='{2}', direccion='{3}', telefono_casa='{4}', telefono_movil='{5}', nombre_contacto='{6}', numero_telefono_contacto='{7}' WHERE dpi='{8}'";
                        queryAlterno = String.Format(queryAlterno, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto, contacto.dpi);

                        dh.GuardarSentenciaEnArchivo(queryAlterno, "pgsql", "I", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), query, contacto.dpi, String.Empty);
                    }

                    // Cierra la conexión
                    this.Close();
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Metodo para modificar un contacto de la base de datos MySQL
        /// </summary>
        /// <param name="dpi">Numero de DPI que identifique al contacto que se desea modificar</param>
        /// <param name="contacto">Modelo de contacto con los posibles nuevos valores</param>
        /// <returns>Cantidad de tuplas afectadas</returns>
        public int ModificarContacto(String dpi, Contacto contacto)
        {

            rowsAffected = 0;

            String query = "UPDATE contacto SET dpi='{0}', nombre='{1}', apellido='{2}', direccion='{3}', telefono_casa='{4}', telefono_movil='{5}', nombre_contacto='{6}', numero_telefono_contacto='{7}' WHERE dpi='{8}'";
            query = String.Format(query, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto, dpi);

            //Open connection
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if (rowsAffected > 0)
                {
                    String queryAlterno = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                    queryAlterno = String.Format(queryAlterno, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto);

                    dh.GuardarSentenciaEnArchivo(query, "pgsql", "M", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), queryAlterno, dpi, contacto.dpi);
                }

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

            String query = String.Format("DELETE FROM contacto WHERE dpi='{0}'", dpi);

            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if (rowsAffected > 0)
                {
                    dh.GuardarSentenciaEnArchivo(query, "pgsql", "D", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), String.Empty, dpi, String.Empty);
                }

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

            String query = "SELECT * FROM contacto";

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

        /// <summary>
        /// Verifica si en la base de datos existe un registro con el dpi enviado
        /// </summary>
        /// <param name="dpi">DPI de la persona que se desea buscar en la base de datos</param>
        /// <returns>Valor booleano que india si el registro existe o no</returns>
        public bool ConsultarRegistro(String dpi)
        {
            dataReader = null;

            String query = String.Format("SELECT * FROM contacto WHERE dpi ='{0}'", dpi);

            //Abre la conexion
            if (this.Open())
            {
                // Crea la sentencia de ejecucion del select
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia y la guarda en el dataReader
                dataReader = cmd.ExecuteReader();

                // Si los resultados no son mayores a 0, no encontro ningun registro con ese dpi
                // retorna falso, de lo contrario, retorna verdadero
                if (!dataReader.HasRows)
                {
                    // Cierra la conexión
                    this.Close();

                    return false;
                }
            }

            // Cierra la conexión
            this.Close();

            return true;
        }

        /// <summary>
        /// Ejecuta el query enviado en MySQL
        /// </summary>
        /// <param name="query">Sentencia a ejecutar en la base de datos MySQL</param>
        /// <returns></returns>
        public bool EjecutarQuery(String query)
        {
            rowsAffected = 0;

            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if(!(rowsAffected > 0))
                {
                    return false;
                }

                // Cierra la conexión
                this.Close();
            }

            return true;
        }

        /// <summary>
        /// Verifica si en la base de datos existe un el usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Passwor de usuario</param>
        /// <returns></returns>
        public bool Login(String username, String password)
        {
            dataReader = null;

            String query = String.Format("SELECT * FROM usuario WHERE usuario ='{0}' AND password = '{1}'", username, password);

            //Abre la conexion
            if (this.Open())
            {
                // Crea la sentencia de ejecucion del select
                cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia y la guarda en el dataReader
                dataReader = cmd.ExecuteReader();

                // Si los resultados no son mayores a 0, no encontro ningun registro con ese dpi
                // retorna falso, de lo contrario, retorna verdadero
                if (!dataReader.HasRows)
                {
                    // Cierra la conexión
                    this.Close();

                    return false;
                }
            }

            // Cierra la conexión
            this.Close();

            return true;
        }

    }
}