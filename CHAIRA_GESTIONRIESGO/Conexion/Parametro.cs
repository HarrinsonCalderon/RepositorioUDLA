using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Conexion
{
    public class Parametro
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public OracleDbType Tipo { get; set; }
        public ParameterDirection Direccion { get; set; }
        public int Ubicacion { get; set; }

        /// <summary>
        /// Crea un parámetro para ser añadido a un OracleCommand cuando se ejecuta un procedimiento
        /// </summary>
        /// <param name="nombre">Nombre del parámetro</param>
        /// <param name="valor">Valor del parámetro</param>
        /// <param name="tipo">Tipo del parámetro. Valores permitidos: CURSOR, NUMBER, VARCHAR2, DATE, CLOB, BLOB</param>
        /// <param name="direccion">Indica si es de entrada o salida</param>
        public Parametro(string nombre, object valor, string tipo, ParameterDirection direccion = ParameterDirection.Input)
        {
            Nombre = nombre;
            Valor = valor;
            Tipo = OraTipo(tipo);
            Direccion = direccion;
        }

        /// <summary>
        /// Crea un parámetro para ser añadido a un OracleCommand, para ejecutar instrucciones Insert, Update, Delete
        /// </summary>
        /// <param name="nombre">Nombre del parámetro</param>
        /// <param name="valor">Valor del parámetro</param>
        /// <param name="tipo">Tipo del parámetro. Valores permitidos: CURSOR, NUMBER, VARCHAR2, DATE, CLOB, BLOB</param>
        /// <param name="ubicacion">El parámetro se usa en un SQL determinado de una transacción</param>
        public Parametro(string nombre, string valor, string tipo, int ubicacion)
        {
            Nombre = nombre;
            Valor = valor;
            Tipo = OraTipo(tipo);
            Ubicacion = ubicacion;
        }

        private OracleDbType OraTipo(string tipo)
        {
            switch (tipo)
            {
                case "CURSOR":
                    return OracleDbType.RefCursor;
                case "NUMBER":
                    return OracleDbType.Int32;
                case "DOUBLE":
                    return OracleDbType.Double;
                case "VARCHAR2":
                    return OracleDbType.Varchar2;
                case "CHAR":
                    return OracleDbType.Char;
                case "NVARCHAR2":
                    return OracleDbType.NVarchar2;
                case "DATE":
                    return OracleDbType.Varchar2;
                case "CLOB":
                    return OracleDbType.Clob;
                case "BLOB":
                    return OracleDbType.Blob;
                default:
                    return OracleDbType.Varchar2;
            }
        }
    }
}