using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Ext.Net;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class General
    {

        public General()
        {

        }

        public string getMoney(double value)
        {
            return value.ToString("C", CultureInfo.CurrentCulture);
        }

        public double getPorcentajeConsumo(double total, double value)
        {
            if (total == 0 && value == 0)
            {
                return 0;
            }
            else
            {
                return ((100 * value) / total);
            }
        }

        //-------------------------------------------------------EXPORT DATATABLE TO EXCEL--------------------------------------------------------------------------------------


        public void ExportDataTableToExcel(DataTable tbl, string filename)
        {

            var wbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = wbook.Worksheets.Add(tbl, "Factura");
            //worksheet.Cell(10, 10).InsertTable(tbl);
            //worksheet.Columns().AdjustToContents();

            HttpResponse httpResponse = HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + ".xlsx\"");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();
        }

        public DataTable ExcelToDataTable(string FileName)
        {
            DataTable dtResult = null;
            int totalSheet = 0; //No of sheets on excel file  
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable()
                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                         select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult; //Returning Dattable  
            }
        }

        public DataTable exceldata(string filePath)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            DataRow schemaRow = schemaTable.Rows[0];
            string sheet = schemaRow["TABLE_NAME"].ToString();
            if (!sheet.EndsWith("_"))
            {
                string query = "SELECT  * FROM [" + sheet + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.CurrentCulture;
                daexcel.Fill(dtexcel);
            }

            conn.Close();
            return dtexcel;

        }

        public DataTable compareHeaderDataTables(DataTable template, DataTable compare)
        {
            DataTable result = new DataTable();
            result.Columns.Add("RESULT");
            result.Columns.Add("COMMENTS");

            string nameColumnTemplate, nameColumnCompare;

            int columnsTemplate, columnsCompare;
            columnsTemplate = template.Columns.Count;
            columnsCompare = compare.Columns.Count;

            if (columnsTemplate != columnsCompare)
            {
                result.Rows.Add("false", "El número de columnas de la tabla ha sido alterado. Tabla original: " + columnsTemplate + ", Tabla nueva: " + columnsCompare);
            }
            else
            {
                for (int i = 0; i < columnsTemplate; i++)
                {
                    nameColumnTemplate = template.Columns[i].ToString();
                    nameColumnCompare = compare.Columns[i].ToString();

                    if (!nameColumnTemplate.Equals(nameColumnCompare))
                    {
                        result.Rows.Add("false", "La columna '" + nameColumnTemplate + "' ha sido cambiada por '" + nameColumnCompare + "'");
                    }
                }
            }

            return result;
        }

        #region Metodos de Optimización de lineas de código

        /// <summary>
        /// Metodo para actualizar los stores
        /// </summary>
        /// <param name="tabla">store</param>
        /// <param name="datos">data table</param>
        public void ActualizarStore(Store tabla, DataTable datos)
        {
            if (!datos.TableName.Equals("_notificacion"))
                tabla.DataSource = datos;
            else
                tabla.RemoveAll();

            tabla.DataBind();
        }

        #endregion

        #region Metodos de manejo de datos

        /// <summary>
        /// Metodo para convertir DataTable a Json
        /// </summary>
        /// <param name="datos">DataTable con los datos a convertir</param>
        /// <returns></returns>
        public string DataTableToJSON(DataTable datos)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in datos.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in datos.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializar = new JavaScriptSerializer();
            string valor = serializar.Serialize(list).ToString();
            return valor != "[]" ? valor : null;
        }

        /// <summary>
        /// Metodo para realizar una consulta a un datatable 
        /// </summary>
        /// <param name="tabla">tabla a consultar</param>
        /// <param name="columna">Columna de referencia</param>
        /// <param name="dato">Dato de consulta</param>
        /// <param name="tipo_atributo">Tipo de columna: entero, decimal o string (default)</param>
        /// <returns>tabla de datos</returns>
        public DataTable ConsultasInterna(DataTable tabla, string columna, string dato, string T_atributo)
        {
            switch (T_atributo)
            {
                case "entero":
                    return (from order in tabla.AsEnumerable()
                            where order.Field<Int32>(columna) == Convert.ToInt32(dato)
                            select order).CopyToDataTable();

                case "decimal":
                    return (from order in tabla.AsEnumerable()
                            where order.Field<decimal>(columna) == Convert.ToDecimal(dato)
                            select order).CopyToDataTable();

                default:
                    return (from order in tabla.AsEnumerable()
                            where order.Field<string>(columna) == dato
                            select order).CopyToDataTable();
            }

        }

        /// <summary>
        /// Editar valor en json una lista simple
        /// </summary>
        /// <param name="json">string del json</param>
        /// <param name="columna"> columna a editar campo </param>
        /// <param name="dato">dato a cambiar</param>
        /// <returns></returns>
        public string EditarValorJSON(string json, string columna, string dato)
        {
            var js = (JObject)JsonConvert.DeserializeObject(json);
            js[columna] = dato;
            return JsonConvert.SerializeObject(js);
        }

        /// <summary>
        /// Metodo para convertir un JSON a DataTable
        /// </summary>
        /// <param name="json">string del json</param>
        /// <returns>DataTable</returns>
        public DataTable JSONToDataTable(string json)
        {
            var jsonLinq = JObject.Parse("{\"datos\":" + json + "}");

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        #endregion

        #region Metodos de mensajeria o de alertas
        public void ShowAlertaInformacion(DataTable mensaje)
        {
            DataRow dr = mensaje.Rows[0];
            string msg = dr["MENSAJE"].ToString(), opcion = dr["TIPO"].ToString();
            AlertaInformacion(msg, opcion);
        }
        /// <summary>
        /// Metodo para mostrar mensaje estandar para cualquier acción
        /// </summary>
        /// <param name="Titulo">Titulo del mensaje</param>
        /// <param name="msg">Mensaje</param>
        /// <param name="opcion">Opciones:Correctamente="Correcto",Información="Informacion",Notificación="Notificacion" y Error="" </param>
        public void AlertaInformacion(string msg, string opcion = "")
        {
            Ext.Net.Icon icono;
            UI Estilo;
            string Titulo = "¡Error!";
            int height = 55;

            #region Height automatico
            int lineas = 0, caracter = msg.Length, salto = Regex.Matches(Regex.Escape(msg), "<br>").Count;
            lineas += caracter / 40;
            lineas += (caracter % 40) > 0 ? 1 : 0;
            height += (lineas * 15) + (salto * 15);
            #endregion

            switch (opcion)
            {
                case "Correcto":
                    Titulo = "¡Proceso exitoso!";
                    icono = Ext.Net.Icon.Accept;
                    Estilo = UI.Success;
                    break;
                case "Informacion":
                    Titulo = "¡Información!";
                    icono = Ext.Net.Icon.Information;
                    Estilo = UI.Primary;
                    break;
                case "Sugerencia":
                    Titulo = "¡Sugerencia!";
                    icono = Ext.Net.Icon.Lightbulb;
                    Estilo = UI.Default;
                    break;
                case "Ayuda":
                    Titulo = "¿Ayuda?";
                    icono = Ext.Net.Icon.Help;
                    Estilo = UI.Info;
                    break;
                case "Notificacion":
                    Titulo = "¡Advertencia!";
                    icono = Ext.Net.Icon.Error;
                    Estilo = UI.Warning;
                    break;

                default:
                    icono = Ext.Net.Icon.Cancel;
                    Estilo = UI.Danger;
                    break;

            }

            Notification.Show(new NotificationConfig
            {
                Title = Titulo,
                Icon = icono,
                PinEvent = "click",
                BodyStyle = "padding:10px; text-align:justify !important;",
                Closable = true,
                Height = height,
                Html = msg,
                Width = 310,
                HideDelay = 4000,
                UI = Estilo
            });

        }


        #endregion
    }
}