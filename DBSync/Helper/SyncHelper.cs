using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using DBSync.Models;
using DBSync.Connection;

namespace DBSync.Helper
{
    public class SyncHelper
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
        /// Instancia a la conexión MySQL
        /// </summary>
        private MySQLConnection conexionMySQL = new MySQLConnection();

        /// <summary>
        /// Instancia a la conexión PostgreSQL
        /// </summary>
        private PostgreSQLConnection conexionPgSQL = new PostgreSQLConnection();

        /// <summary>
        /// Ejecuta la sincronización en las dos bases de datos
        /// </summary>
        /// <returns></returns>
        public bool SincronizarBaseDeDatos()
        {
            String pathFile = HttpContext.Current.Server.MapPath(FILE_LOG_NAME);
            String pathFileLog = HttpContext.Current.Server.MapPath(LOG_PATH + FILE_LOG_NAME);
            String pathFileLogBackup = HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH + FILE_LOG_NAME);

            if (File.Exists(pathFile))
            {
                this.ProcesoSincronizacion(pathFile);
            }
            else if (File.Exists(pathFileLog))
            {
                this.ProcesoSincronizacion(pathFileLog);
            }
            else if (File.Exists(pathFileLogBackup))
            {
                this.ProcesoSincronizacion(pathFileLogBackup);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Realiza todos los procesos internos de ordenamiento de la información y solicitud de ejecución de sentencias en ambas
        /// bases de datos para que la sincronización se lleve a cabo.
        /// </summary>
        /// <param name="pathFile">Ruta del archivo donde se intentará leer los logs</param>
        /// <returns></returns>
        private bool ProcesoSincronizacion(String pathFile)
        {
            EncryptHelper encripter = new EncryptHelper();

            List<String> listaSentencias = new List<String>();
            List<Query> listaQuerys = new List<Query>();

            try
            {
                // Lee todas las filas del archivo log
                using (StreamReader sr = new StreamReader(pathFile))
                {
                    String linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        listaSentencias.Add(encripter.Decrypt(linea));
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
                    query.DpiOriginal = palabras[6] ?? String.Empty;
                    query.DpiModificado = palabras[7] ?? String.Empty;

                    listaQuerys.Add(query);
                }

                // Ordena la lista de querys por la hora en la que fueron ejecutados
                listaQuerys.OrderBy(q => q.Hora);

                foreach (Query query in listaQuerys)
                {
                    if (query.Pendiente == String.Empty)
                    {
                        switch (query.TipoQuery)
                        {
                            case "I":
                                // Verifica si el registro existe o no en ambas bases de datos
                                if (conexionMySQL.ConsultarRegistro(query.DpiOriginal))
                                {
                                    // Si el usuario existe, ejecuta la consulta especifica
                                    conexionMySQL.EjecutarQuery(query.QueryString);
                                }
                                else
                                {
                                    // De lo contrario, ejecutará la consulta alternativa
                                    conexionMySQL.EjecutarQuery(query.QueryAlterno);
                                }

                                if (conexionPgSQL.ConsultarRegistro(query.DpiOriginal))
                                {
                                    // Si el usuario existe, ejecuta la consulta especifica
                                    conexionPgSQL.EjecutarQuery(query.QueryString);
                                }
                                else
                                {
                                    // De lo contrario, ejecutará la consulta alternativa
                                    conexionPgSQL.EjecutarQuery(query.QueryAlterno);
                                }
                                break;
                            case "M":
                                // Si el DPI nuevo y el anterior no cambian, solo se actualiza o inserta el registro en ambas bases
                                if (query.DpiOriginal == query.DpiModificado)
                                {
                                    // Verifica si el registro existe o no en ambas bases de datos
                                    if (conexionMySQL.ConsultarRegistro(query.DpiOriginal))
                                    {
                                        // Si el usuario existe, hace un Update
                                        conexionMySQL.EjecutarQuery(query.QueryString);
                                    }
                                    else
                                    {
                                        // De lo contrario, hace un Insert
                                        conexionMySQL.EjecutarQuery(query.QueryAlterno);
                                    }

                                    if (conexionPgSQL.ConsultarRegistro(query.DpiOriginal))
                                    {
                                        // Si el usuario existe, hace un Update
                                        conexionPgSQL.EjecutarQuery(query.QueryString);
                                    }
                                    else
                                    {
                                        // De lo contrario, hace un Insert
                                        conexionPgSQL.EjecutarQuery(query.QueryAlterno);
                                    }
                                }
                                // Si no
                                else
                                {
                                    // Si el DPI original existe en la base de datos
                                    if (conexionMySQL.ConsultarRegistro(query.DpiOriginal))
                                    {
                                        // Si el DPI nuevo existe en la base de datos
                                        if (conexionMySQL.ConsultarRegistro(query.DpiModificado))
                                        {
                                            // Elimina el registro del DPI viejo
                                            conexionMySQL.EjecutarQuery(String.Format("DELETE FROM contacto WHERE dpi='{0}'", query.DpiOriginal));

                                            // Hace un update al registro del DPI nuevo
                                            String newQuery = String.Format(query.QueryString.Substring(0, query.QueryString.IndexOf("WHERE")) + "WHERE dpi='{0}'", query.DpiModificado);
                                            conexionMySQL.EjecutarQuery(newQuery);
                                        }
                                        // si no, solo se hace el update normal
                                        else
                                        {
                                            conexionMySQL.EjecutarQuery(query.QueryString);
                                        }
                                    }
                                    // de lo contrario, Inserta (o hace un Update) del registro con el nuevo DPI
                                    else
                                    {
                                        // Si el nuevo DPI existe, se hace un Update
                                        if (conexionMySQL.ConsultarRegistro(query.DpiModificado))
                                        {
                                            String newQuery = String.Format(query.QueryString.Substring(0, query.QueryString.IndexOf("WHERE")) + "WHERE dpi='{0}'", query.DpiModificado);
                                            conexionMySQL.EjecutarQuery(newQuery);
                                        }
                                        // de lo contrario, solo se inserta el nuevo registro
                                        else
                                        {
                                            // De lo contrario, hace un Insert
                                            conexionMySQL.EjecutarQuery(query.QueryAlterno);
                                        }
                                    }

                                    if (conexionPgSQL.ConsultarRegistro(query.DpiOriginal))
                                    {
                                        // Si el DPI nuevo existe en la base de datos
                                        if (conexionPgSQL.ConsultarRegistro(query.DpiModificado))
                                        {
                                            // Elimina el registro del DPI viejo
                                            conexionPgSQL.EjecutarQuery(String.Format("DELETE FROM contacto WHERE dpi='{0}'", query.DpiOriginal));

                                            // Hace un update al registro del DPI nuevo
                                            String newQuery = String.Format(query.QueryString.Substring(0, query.QueryString.IndexOf("WHERE")) + "WHERE dpi='{0}'", query.DpiModificado);
                                            conexionPgSQL.EjecutarQuery(newQuery);
                                        }
                                        // si no, solo se hace el update normal
                                        else
                                        {
                                            conexionPgSQL.EjecutarQuery(query.QueryString);
                                        }
                                    }
                                    // de lo contrario, Inserta (o hace un Update) del registro con el nuevo DPI
                                    else
                                    {
                                        // Si el nuevo DPI existe, se hace un Update
                                        if (conexionPgSQL.ConsultarRegistro(query.DpiModificado))
                                        {
                                            String newQuery = String.Format(query.QueryString.Substring(0, query.QueryString.IndexOf("WHERE")) + "WHERE dpi='{0}'", query.DpiModificado);
                                            conexionPgSQL.EjecutarQuery(newQuery);
                                        }
                                        // de lo contrario, solo se inserta el nuevo registro
                                        else
                                        {
                                            // De lo contrario, hace un Insert
                                            conexionPgSQL.EjecutarQuery(query.QueryAlterno);
                                        }
                                    }
                                }
                                break;
                            case "D":
                                // Verifica si el registro existe o no en ambas bases de datos
                                if (conexionMySQL.ConsultarRegistro(query.DpiOriginal))
                                {
                                    // Si el usuario existe, ejecuta la consulta especifica
                                    conexionMySQL.EjecutarQuery(query.QueryString);
                                }

                                if (conexionPgSQL.ConsultarRegistro(query.DpiOriginal))
                                {
                                    // Si el usuario existe, ejecuta la consulta especifica
                                    conexionPgSQL.EjecutarQuery(query.QueryString);
                                }
                                break;
                        }
                    }
                }

                // Inserta los logs dentro del archivo de los archivos de texto de respaldo
                using (StreamWriter swLog = new StreamWriter(HttpContext.Current.Server.MapPath(LOG_PATH + FILE_LOG_NAME), false))
                {
                    foreach (Query query in listaQuerys)
                    {
                        swLog.WriteLine(encripter.Encrypt(String.Format("x|{0}|{1}|{2}|{3}|{4}|{5}|{6}", query.Hora, query.TargetDatabase, query.TipoQuery, query.QueryString, query.QueryAlterno, query.DpiOriginal, query.DpiModificado)));
                    }
                }

                using (StreamWriter swBackup = new StreamWriter(HttpContext.Current.Server.MapPath(LOG_BACKUP_PATH + FILE_LOG_NAME), false))
                {
                    foreach (Query query in listaQuerys)
                    {
                        swBackup.WriteLine(encripter.Encrypt(String.Format("x|{0}|{1}|{2}|{3}|{4}|{5}|{6}", query.Hora, query.TargetDatabase, query.TipoQuery, query.QueryString, query.QueryAlterno, query.DpiOriginal, query.DpiModificado)));
                    }
                }

                using (StreamWriter swPrincipal = new StreamWriter(HttpContext.Current.Server.MapPath(FILE_LOG_NAME), false))
                {
                    foreach (Query query in listaQuerys)
                    {
                        swPrincipal.WriteLine(encripter.Encrypt(String.Format("x|{0}|{1}|{2}|{3}|{4}|{5}|{6}", query.Hora, query.TargetDatabase, query.TipoQuery, query.QueryString, query.QueryAlterno, query.DpiOriginal, query.DpiModificado)));
                    }
                }

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}