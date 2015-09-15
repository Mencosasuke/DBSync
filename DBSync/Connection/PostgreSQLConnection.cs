﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DBSync.Models;
using DBSync.Helper;

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
        /// Variable de ejecución de comandos
        /// </summary>
        private NpgsqlCommand cmd;

        /// <summary>
        /// Variable donde se guarda el resultado de las consultas select
        /// </summary>
        private NpgsqlDataReader dataReader;

        /// <summary>
        /// Variable que guarda la sentencia a ejecutar
        /// </summary>
        private String query;

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
        public PostgreSQLConnection()
        {
            conexion = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
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
        private bool Close()
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
        /// Metodo para hacer un insert a la base de datos PostgreSQL
        /// </summary>
        /// <returns></returns>
        public int InsertarContacto(Contacto contacto)
        {
            rowsAffected = 0;

            query = String.Empty;
            query = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            query = String.Format(query, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto);

            // Abre la conexión
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();
                Debug.WriteLine("Sentencia ejecutada exitosamente");

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if (rowsAffected > 0)
                {
                    String queryAlterno = "UPDATE contacto SET dpi='{0}', nombre='{1}', apellido='{2}', direccion='{3}', telefono_casa='{4}', telefono_movil='{5}', nombre_contacto='{6}', numero_telefono_contacto='{7}' WHERE dpi='{8}'";
                    queryAlterno = String.Format(queryAlterno, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto, contacto.dpi);

                    dh.GuardarSentenciaEnArchivo(queryAlterno, "mysql", "I", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), query);
                }

                // Cierra la conexión
                this.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Metodo para modificar un contacto de la base de datos PostrgreSQL
        /// </summary>
        /// <param name="dpi">Numero de DPI que identifique al contacto que se desea modificar</param>
        /// <param name="contacto">Modelo de contacto con los posibles nuevos valores</param>
        /// <returns>Cantidad de tuplas afectadas</returns>
        public int ModificarContacto(String dpi, Contacto contacto)
        {

            rowsAffected = 0;

            query = String.Empty;
            query = "UPDATE contacto SET dpi='{0}', nombre='{1}', apellido='{2}', direccion='{3}', telefono_casa='{4}', telefono_movil='{5}', nombre_contacto='{6}', numero_telefono_contacto='{7}' WHERE dpi='{8}'";
            query = String.Format(query, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto, dpi);

            //Open connection
            if (this.Open())
            {
                // Crea la sentencia de ejecución del query
                cmd = new NpgsqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if (rowsAffected > 0)
                {
                    String queryAlterno = "INSERT INTO contacto (dpi, nombre, apellido, direccion, telefono_casa, telefono_movil, nombre_contacto, numero_telefono_contacto) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                    queryAlterno = String.Format(queryAlterno, contacto.dpi, contacto.nombre, contacto.apellido, contacto.direccion, contacto.telefonoCasa, contacto.telefonoMovil, contacto.nombreContacto, contacto.numeroContacto);

                    dh.GuardarSentenciaEnArchivo(query, "mysql", "M", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), queryAlterno);
                }

                // Cierrla la conexión
                this.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Metodo para eliminar un contacto de la base de datos PostrgreSQL
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
                cmd = new NpgsqlCommand(query, conexion);

                // Ejecuta la sentencia
                rowsAffected = cmd.ExecuteNonQuery();

                // Si la sentencia se ejecuta correctamente, se arma la linea a guardar en el log de sentencias
                if (rowsAffected > 0)
                {
                    dh.GuardarSentenciaEnArchivo(query, "mysql", "D", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), String.Empty);
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
        public NpgsqlDataReader ObtenerRegistros()
        {
            dataReader = null;

            query = String.Empty;
            query = "SELECT * FROM contacto";

            // Abre la conexion
            if (this.Open())
            {
                // Crea la sentencia de ejecucion del select
                cmd = new NpgsqlCommand(query, conexion);

                // Ejecuta la sentencia y la guarda en el dataReader
                dataReader = cmd.ExecuteReader();
            }

            return dataReader;
        }

    }
}