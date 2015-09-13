﻿using System;
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
        /// Metodo para hacer un insert a la DB
        /// </summary>
        /// <returns></returns>
        public int insertarContacto()
        {
            int rowsAffected = 0;

            String query = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            query = String.Format(query, "2567648320101", "David", "Mencos", "Mi Casa", "numero1", "numero2", "Contacto 1", "numero 3");

            // Abre conexión
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                MySqlCommand cmd = new MySqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();
                Debug.WriteLine("Sentencia ejecutada exitosamente");

                // Cierra la conexión
                this.Close();
            }

            return rowsAffected;
        }
    }
}