using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MySql.Data.MySqlClient;
using Npgsql;

using DBSync.Models;

namespace DBSync.Helper
{
    public class DataHelper
    {

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
    }
}