using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Conexion
{
    public class CProcedimientos
    {
        public string EjecutarOperacion(string procedimiento, List<Parametro> parametros, string conexion = "conexion_administrativo")
        {
            OracleConnection Conexion = new OracleConnection(ConfigurationManager.ConnectionStrings[conexion].ConnectionString);
            string Return = "NO";
            int c = 0;
            try
            {
                #region Define Procedimiento
                DataTable datos = new DataTable();
                Conexion.Open();
                OracleCommand DbCommand = new OracleCommand(procedimiento, Conexion);
                DbCommand.CommandType = CommandType.StoredProcedure;
                #endregion Define Procedimiento

                #region Parametros
                foreach (Parametro p in parametros)
                {
                    if (p.Direccion == ParameterDirection.ReturnValue)
                        DbCommand.Parameters.Insert(0, new OracleParameter(p.Nombre, p.Tipo));
                    else DbCommand.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));

                    DbCommand.Parameters[p.Nombre].Direction = p.Direccion;
                    if (p.Direccion == ParameterDirection.Input)
                        DbCommand.Parameters[p.Nombre].Value = p.Valor;
                    else
                    {
                        DbCommand.Parameters[p.Nombre].Size = 10000;
                        DbCommand.Parameters[p.Nombre].Value = p.Valor;
                    }

                }
                #endregion

                c = DbCommand.ExecuteNonQuery();
                Return = "yes";
            }
            catch (Exception ex)
            {
                //Return = ex.Message;
                throw ex;
                //X.Msg.Alert("Error", " " + ex.Message.Remove(150)).Show();
            }
            finally
            {
                if (Conexion != null)
                    if (Conexion.State == ConnectionState.Open)
                        Conexion.Close();
            }
            return Return;
        }

        public DataTable EjecutarSelect(string procedimiento, List<Parametro> parametros, string conexion = "POAIOADConexion")
        {
            OracleConnection Conexion = new OracleConnection(ConfigurationManager.ConnectionStrings[conexion].ConnectionString);
            DataTable datos = new DataTable();
            try
            {
                #region Define Procedimiento
                OracleDataReader cursor;
                Conexion.Open();
                OracleCommand DbCommand = new OracleCommand(procedimiento, Conexion);
                DbCommand.CommandType = CommandType.StoredProcedure;
                #endregion Define Procedimiento

                #region Parametros
                foreach (Parametro p in parametros)
                {
                    if (p.Direccion == ParameterDirection.ReturnValue)
                        DbCommand.Parameters.Insert(0, new OracleParameter(p.Nombre, p.Tipo));
                    else
                        DbCommand.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));

                    DbCommand.Parameters[p.Nombre].Direction = p.Direccion;
                    if (p.Direccion == ParameterDirection.Input)
                        DbCommand.Parameters[p.Nombre].Value = p.Valor;

                }
                #endregion

                cursor = DbCommand.ExecuteReader();

                datos.Load(cursor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conexion != null)
                    if (Conexion.State == ConnectionState.Open)
                        Conexion.Close();
            }
            return datos;
        }

        public DataSet ProcedureMultiSelectAcademico(string procedimiento, List<Parametro> parametros, string conexion = "conexion_administrativo")
        {
            OracleConnection DbConn = new OracleConnection(ConfigurationManager.ConnectionStrings[conexion].ConnectionString);
            DataTable datos = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                #region Define Procedimiento
                OracleDataReader cursor;
                DbConn.Open();
                OracleCommand DbCommand = new OracleCommand(procedimiento, DbConn);
                DbCommand.CommandType = CommandType.StoredProcedure;
                #endregion Define Procedimiento

                #region Parametros
                foreach (Parametro p in parametros)
                {

                    if (p.Direccion == ParameterDirection.ReturnValue)
                        DbCommand.Parameters.Insert(0, new OracleParameter(p.Nombre, p.Tipo));
                    else DbCommand.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));

                    DbCommand.Parameters[p.Nombre].Direction = p.Direccion;
                    if (p.Direccion == ParameterDirection.Input)
                        DbCommand.Parameters[p.Nombre].Value = p.Valor;

                }
                #endregion

                cursor = DbCommand.ExecuteReader();

                #region Obtener resultados
                foreach (Parametro p in parametros)
                {
                    if (p.Direccion == ParameterDirection.Output)
                    {
                        datos = new DataTable();
                        datos.Load(cursor);
                        datos.TableName = p.Nombre;
                        ds.Tables.Add(datos.Copy());
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (DbConn != null)
                    if (DbConn.State == ConnectionState.Open)
                        DbConn.Close();
            }
            return ds;
        }

    }
}