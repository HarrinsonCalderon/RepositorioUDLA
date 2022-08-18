using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;


namespace CHAIRA_GESTIONRIESGO.Conexion
{
    public class DBOracle
    {
        public string schema = "admin";
        //prueba para agregar
        public string Conection(string schema)
        {

            //return ConfigurationManager.ConnectionStrings["AmazonPRUEBA90"].ConnectionString;
            schema = schema.ToLower();
            switch (schema)
            {
                case "admin":
                    return ConfigurationManager.ConnectionStrings["POAIOADConexion"].ConnectionString;
                case "siif":
                    return ConfigurationManager.ConnectionStrings["SIIF"].ConnectionString;
                case "ciarp":
                    return ConfigurationManager.ConnectionStrings["CIARP"].ConnectionString;
                default:
                    return ConfigurationManager.ConnectionStrings["conexion_administrativo"].ConnectionString;

            }
            //return ConfigurationManager.ConnectionStrings["AmazonPRUEBA90"].ConnectionString;
            //return ConfigurationManager.ConnectionStrings["AlmacenPrueba"].ConnectionString;

            //return ConfigurationManager.ConnectionStrings["conexion_administrativo"].ConnectionString;
            //return "Data Source=172.16.31.62/chaira;uid=ALMACENOAD;Password=ALMACEN2017;";

        }
        public DataSet ProcedureMultiSelectAcademico(string procedimiento, List<Parametro> parametros, string schema = "admin")
        {
            OracleConnection DbConn = new OracleConnection(this.Conection(schema));
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
                if (cursor != null)
                    foreach (Parametro p in parametros)
                    {
                        if (p.Direccion == ParameterDirection.Output || p.Direccion == ParameterDirection.ReturnValue)
                        {
                            datos = new DataTable();
                            datos.Load(cursor);
                            datos.TableName = p.Nombre;
                            ds.Tables.Add(datos.Copy());
                        }
                    }
                else
                    ds = null;
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


        public DataTable OraProcedimiento(string procedimiento, List<Parametro> parametros, string schema = "admin")
        // OraProcedimiento: procesar procedimientos con funciones y retorna DataTable
        {
            OracleConnection connection = new OracleConnection(this.Conection(schema)); //Instanciación conexión
            connection.Open();                                                    //Apertura de conexión      
            OracleCommand comando = new OracleCommand(procedimiento, connection); //Instanciación del comando
            comando.CommandType = CommandType.StoredProcedure;                    //Asignación del tipo de comando

            DataTable datos = new DataTable();                                    //DataTable para alamacenar resultado
            //OracleDataReader dataReader;                                          //Oracle Data Reader para 

            foreach (Parametro p in parametros)
            {
                if (p.Direccion == ParameterDirection.ReturnValue)
                    comando.Parameters.Insert(0, new OracleParameter(p.Nombre, p.Tipo));
                else comando.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));

                comando.Parameters[p.Nombre].Direction = p.Direccion;
                if (p.Direccion == ParameterDirection.Input)
                    comando.Parameters[p.Nombre].Value = p.Valor;
            }
            try
            {

                datos.Load(comando.ExecuteReader());                              //Captura en el DataTable del resultado del procedimiento/función

                if (datos.Columns[0].ColumnName.Equals("_TIPO"))
                {
                    datos.TableName = "_notificacion";
                    return NotificacionCOD.notificar(datos);
                }
                else
                    return datos;                                                   //Retorna el DataTable Capturado
            }
            catch (Exception ex)
            {
                string[] mensaje = ex.Message.Split(new Char[] { '*' });               // Se dividen los diferentes componentes del mensaje
                if (mensaje.Length == 4)
                {
                    datos.Clear();                                                    // Se limpia el DataTable para crear el mensaje
                    datos.TableName = "_notificacion";
                    datos.Columns.Add("_TIPO", typeof(string));                       // Se crean las columnas del DataTable
                    datos.Columns.Add("_TITULO", typeof(string));
                    datos.Columns.Add("_MENSAJE", typeof(string));
                    datos.Rows.Add(                                                     // Se adiciona la fila con el mensaje  
                        mensaje[0],     //tipo
                        mensaje[1],     //titulo
                        mensaje[2]);    //mensaje
                }
                else
                {
                    datos.Clear();                                                    // Se limpia el DataTable para crear el mensaje
                    datos.TableName = "_notificacion";
                    datos.Columns.Add("_TIPO", typeof(string));                       // Se crean las columnas del DataTable
                    datos.Columns.Add("_TITULO", typeof(string));
                    datos.Columns.Add("_MENSAJE", typeof(string));
                    var ErrorSintaxis = (ex.Message.Contains("ORA-20333")) ?
                        "<hr style='border-color: red;'><strong>Estructura del mensaje de excepción:\n</strong>" +
                        "[tipoNotificación]*[tituloNotificación]*[mensajeNotificación]" : "";

                    var mensajeError = "<strong> Mensaje de Excepción:</strong>\n" + ex.Message + ErrorSintaxis;

                    datos.Rows.Add(                                                   // Se adiciona la fila con el mensaje 
                        "errorPin",                                             //tipo
                        "Error en el procedimiento",                                  //titulo
                        mensajeError); //mensaje                       
                }
                return NotificacionCOD.notificar(datos);                                                     //Retorna el DataTable con la Excepción  
            }
            finally
            {
                if (connection != null)
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
            }
        }

        public DataTable OraProcedimiento2(string procedimiento, List<Parametro> parametros, bool notificar = true)
        // OraProcedimiento: procesar procedimientos con funciones y retorna DataTable
        {
            OracleConnection connection = new OracleConnection(this.Conection("")); //Instanciación conexión
            connection.Open();                                                  //Apertura de conexión      
            OracleCommand comando = new OracleCommand(procedimiento, connection); //Instanciación del comando
            comando.CommandType = CommandType.StoredProcedure;                    //Asignación del tipo de comando

            DataTable datos = new DataTable();                                    //DataTable para alamacenar resultado
            //OracleDataReader dataReader;                                          //Oracle Data Reader para 

            foreach (Parametro p in parametros)
            {
                if (p.Direccion == ParameterDirection.ReturnValue)
                    comando.Parameters.Insert(0, new OracleParameter(p.Nombre, p.Tipo));
                else comando.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));

                comando.Parameters[p.Nombre].Direction = p.Direccion;
                if (p.Direccion == ParameterDirection.Input)
                    comando.Parameters[p.Nombre].Value = p.Valor;
            }
            try
            {

                datos.Load(comando.ExecuteReader());                              //Captura en el DataTable del resultado del procedimiento/función

                if (datos.Columns[0].ColumnName.Equals("_TIPO"))
                {
                    datos.TableName = "_notificacion";
                    if (notificar)
                    {
                        return NotificacionCOD.notificar(datos);
                    }
                    else
                    {
                        return datos;
                    }

                }
                else
                    return datos;                                                   //Retorna el DataTable Capturado
            }
            catch (Exception ex)
            {
                string[] mensaje = ex.Message.Split(new Char[] { '*' });               // Se dividen los diferentes componentes del mensaje
                if (mensaje.Length == 4)
                {
                    datos.Clear();                                                    // Se limpia el DataTable para crear el mensaje
                    datos.TableName = "_notificacion";
                    datos.Columns.Add("_TIPO", typeof(string));                       // Se crean las columnas del DataTable
                    datos.Columns.Add("_TITULO", typeof(string));
                    datos.Columns.Add("_MENSAJE", typeof(string));
                    datos.Rows.Add(                                                     // Se adiciona la fila con el mensaje  
                        mensaje[0],     //tipo
                        mensaje[1],     //titulo
                        mensaje[2]);    //mensaje
                }
                else
                {
                    datos.Clear();                                                    // Se limpia el DataTable para crear el mensaje
                    datos.TableName = "_notificacion";
                    datos.Columns.Add("_TIPO", typeof(string));                       // Se crean las columnas del DataTable
                    datos.Columns.Add("_TITULO", typeof(string));
                    datos.Columns.Add("_MENSAJE", typeof(string));
                    var ErrorSintaxis = (ex.Message.Contains("ORA-20333")) ?
                        "<hr style='border-color: red;'><strong>Estructura del mensaje de excepción:\n</strong>" +
                        "[tipoNotificación]*[tituloNotificación]*[mensajeNotificación]" : "";

                    var mensajeError = "<strong> Mensaje de Excepción:</strong>\n" + ex.Message + ErrorSintaxis;

                    datos.Rows.Add(                                                   // Se adiciona la fila con el mensaje 
                        "errorPin",                                             //tipo
                        "Error en el procedimiento",                                  //titulo
                        mensajeError); //mensaje                       
                }
                if (notificar)
                {
                    return NotificacionCOD.notificar(datos);
                }
                else
                {
                    return datos;
                }                                                     //Retorna el DataTable con la Excepción  
            }
            finally
            {
                if (connection != null)
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
            }
        }


        internal DataSet ProcedureMultiSelectAcademico(string procedimiento, List<Parametro> parametros)
        {
            OracleConnection DbConn = new OracleConnection(this.Conection(schema));
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

        public DataTable EjecutarProcedimiento(string procedimiento, List<Parametro> parametros, string instanciaBD = "chaira")
        {
            string cadenaConexion = (instanciaBD.ToLower() != "siif") ? "admin" : "siif";
            return ProcedureMultiSelectAcademico(procedimiento, parametros, cadenaConexion).Tables[0];
        }

        public string OraSqlQueryString(string SQL)
        {
            OracleConnection conec = new OracleConnection(this.Conection("ADMINISTRATIVO"));
            OracleCommand comando = new OracleCommand(SQL, conec);
            OracleDataAdapter datos = new OracleDataAdapter(comando);
            DataSet tabla = new DataSet();
            try
            {
                conec.Open();
                datos.Fill(tabla);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conec.Close();
                comando.Dispose();
            }
            return (tabla.Tables[0].Rows[0][0]).ToString();
        }

        public string ProcedureIUString(string procedimiento, List<Parametro> parametros, string con = "")
        {
            OracleConnection DbConn = new OracleConnection(this.Conection(con));
            string Return = "NO";
            int c = 0;
            DbConn.Open();
            OracleCommand DbCommand = new OracleCommand(procedimiento, DbConn);
            try
            {
                #region Define Procedimiento
                DataTable datos = new DataTable();

                DbCommand.CommandType = CommandType.Text;
                #endregion Define Procedimiento

                #region Parametros
                //foreach (Parametro p in parametros)
                //{
                //    DbCommand.Parameters.Add(new OracleParameter(p.Nombre, p.Tipo));
                //    DbCommand.Parameters[p.Nombre].Direction = p.Direccion;
                //    //if (p.Direccion == ParameterDirection.Output)
                //    //    DbCommand.Parameters[p.Nombre].Size = 100000;
                //    DbCommand.Parameters[p.Nombre].Value = p.Valor;
                //}
                #endregion

                //c = DbCommand.ExecuteNonQuery();
                object df = DbCommand.ExecuteScalar();
                return df.ToString();
                //Return = "yes";
            }
            catch (Exception ex)
            {
                Return = ex.Message;
            }
            finally
            {
                if (DbConn != null)
                    if (DbConn.State == ConnectionState.Open)
                    {
                        //OracleConnection.ClearPool(DbConn);
                        DbConn.Close();
                        DbCommand.Dispose();
                    }
            }
            return Return;
        }
    }
}