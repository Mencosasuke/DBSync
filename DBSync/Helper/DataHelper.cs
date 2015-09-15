using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using MySql.Data.MySqlClient;
using Npgsql;

using DBSync.Models;
using DBSync.Connection;

namespace DBSync.Helper
{
    public class DataHelper
    {

        /// <summary>
        /// Path a la carpeta Logs
        /// </summary>
        private static String LOG_PATH = "~/Logs";

        /// <summary>
        /// Path a la carpeta de backup
        /// </summary>
        private static String LOG_BACKUP_PATH = "~/Resources/Backup";

        /// <summary>
        /// Path al archivo de log
        /// </summary>
        private static String FILE_LOG_NAME = "/queryLogs.txt";

        /// <summary>
        /// Arma la lista de contactos con la información obtenida de la DB de MySQL
        /// </summary>
        /// <param name="data">DataReader obtenido de la consulta select</param>
        /// <returns>Lista de los contactos con su información respectiva</returns>
        public List<Contacto> ArmarListaContactosMySQL(MySqlDataReader data)
        {
            List<Contacto> listaContactos = new List<Contacto>();

            // Lee la informacion y la almacena en cada campo del modelo de vista
            while (data.Read())
            {
                Contacto contacto = new Contacto();

                contacto.dpi = (String)data["dpi"] ?? String.Empty;
                contacto.nombre = (String)data["nombre"] ?? String.Empty;
                contacto.apellido = (String)data["apellido"] ?? String.Empty;
                contacto.direccion = (String)data["direccion"] ?? String.Empty;
                contacto.telefonoCasa = (String)data["telefono_casa"] ?? String.Empty;
                contacto.telefonoMovil = (String)data["telefono_movil"] ?? String.Empty;
                contacto.nombreContacto = (String)data["nombre_contacto"] ?? String.Empty;
                contacto.numeroContacto = (String)data["numero_telefono_contacto"] ?? String.Empty;

                listaContactos.Add(contacto);
            }

            return listaContactos;
        }

        /// <summary>
        /// Arma la lista de contactos con la información obtenida de la DB de PostgreSQL
        /// </summary>
        /// <param name="data">DataReader obtenido de la consulta select</param>
        /// <returns>Lista de los contactos con su información respectiva</returns>
        public List<Contacto> ArmarListaContactosPgSQL(NpgsqlDataReader data)
        {
            List<Contacto> listaContactos = new List<Contacto>();

            // Lee la información y la almacena en cada campo del modelo de vista
            while (data.Read())
            {
                Contacto contacto = new Contacto();

                contacto.dpi = (String)data["dpi"] ?? String.Empty;
                contacto.nombre = (String)data["nombre"] ?? String.Empty;
                contacto.apellido = (String)data["apellido"] ?? String.Empty;
                contacto.direccion = (String)data["direccion"] ?? String.Empty;
                contacto.telefonoCasa = (String)data["telefono_casa"] ?? String.Empty;
                contacto.telefonoMovil = (String)data["telefono_movil"] ?? String.Empty;
                contacto.nombreContacto = (String)data["nombre_contacto"] ?? String.Empty;
                contacto.numeroContacto = (String)data["numero_telefono_contacto"] ?? String.Empty;

                listaContactos.Add(contacto);
            }

            return listaContactos;
        }


        /// <summary>
        /// Guarda en archivos .txt los logs de las consultas realizadas en cualquiera de las dos bases de datos.
        /// </summary>
        /// <param name="query">Sentencia a ejecutar en la base de datos</param>
        /// <param name="targetDatabase">Base de datos en la cual se debe ejecutar el query</param>
        /// <param name="tipoQuery">Indica si el query fue un Select, Update o Delete</param>
        /// <param name="timeStamp">Hora exacta en la que se finalizó la consulta</param>
        /// <param name="queryAlterno">Query alterno que se debe ejecutar si el registro existe o no</param>
        /// <returns></returns>
        public bool GuardarSentenciaEnArchivo(String query, String targetDatabase, String tipoQuery, String timeStamp, String queryAlterno, String dpiOriginal, String dpiModificado)
        {
            // Si el directorio no existe, lo crea
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(LOG_PATH)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(LOG_PATH));
            }

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH));
            }

            // Inserta los logs dentro del archivo de los archivos de texto de respaldo
            using (StreamWriter swLog = File.AppendText(HttpContext.Current.Server.MapPath(LOG_PATH + FILE_LOG_NAME)))
            {
                swLog.WriteLine(String.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6};", timeStamp, targetDatabase, tipoQuery, query, queryAlterno, dpiOriginal, dpiModificado));
            }

            using (StreamWriter swBackup = File.AppendText(HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH + FILE_LOG_NAME)))
            {
                swBackup.WriteLine(String.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6};", timeStamp, targetDatabase, tipoQuery, query, queryAlterno, dpiOriginal, dpiModificado));
            }

            using (StreamWriter swPrincipal = File.AppendText(HttpContext.Current.Server.MapPath(FILE_LOG_NAME)))
            {
                swPrincipal.WriteLine(String.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6};", timeStamp, targetDatabase, tipoQuery, query, queryAlterno, dpiOriginal, dpiModificado));
            }

            return true;
        }

        public bool SincronizarBaseDeDatos()
        {
            List<String> listaSentencias = new List<String>();
            List<Query> listaQuerys = new List<Query>();
            
            String pathFile = HttpContext.Current.Server.MapPath(FILE_LOG_NAME);
            String pathFileLog = HttpContext.Current.Server.MapPath(LOG_PATH + FILE_LOG_NAME);
            String pathFileLogBackup = HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH + FILE_LOG_NAME);

            if (File.Exists(pathFile))
            {
                try
                {
                    // Lee todas las filas del archivo log
                    using (StreamReader sr = new StreamReader(pathFile))
                    {
                        String linea;
                        while ((linea = sr.ReadLine()) != null)
                        {
                            listaSentencias.Add(linea);
                        }
                    }

                    // Arma la lista de querys que se deben (o no) ejecutar
                    foreach (String registro in listaSentencias)
                    {
                        Query query = new Query();

                        String[] palabras = registro.Split('|');

                        query.Pendiente = palabras[0] ?? String.Empty;
                        query.Hora = palabras[1] ?? String.Empty;
                        query.TargetDatabase = palabras[2] ?? String.Empty;
                        query.TipoQuery = palabras[3] ?? String.Empty;
                        query.QueryString = palabras[4] ?? String.Empty;
                        query.QueryAlterno = palabras[5] ?? String.Empty;

                        listaQuerys.Add(query);
                    }

                    // Ejecuta las sentencias en la DB correspondiente según la información obtenida de los logs
                    //foreach (Query query in listaQuerys)
                    //{

                    //}
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else if (File.Exists(pathFileLog))
            {

            }
            else if (File.Exists(pathFileLogBackup))
            {

            }
            else
            {
                return false;
            }
            
            return true;
        }
    }
}