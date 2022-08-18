using CHAIRA_GESTIONRIESGO.Controlador;
using CHAIRA_GESTIONRIESGO.Utilities;
using ChairaMongo;
using Ext.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
{
    public partial class FormularioPrueba : System.Web.UI.Page
    {
        CEjemplo cReporte = new CEjemplo();
        //MongoNoSQL MG = new MongoNoSQL("NombreEsquemaMongo", "172.16.31.19:27017");
        MongoNoSQL MG = new MongoNoSQL("DocumentosPOAI", "172.16.31.19:27017");
        PDFFile pdf = new PDFFile();
        Util cUtil = new Util();
        string _pege_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            _pege_id = cUtil.GetPege();
            if (!X.IsAjaxRequest)
            {
                this.datos_reportActividad();
            }
        }

        public void datos_reportActividad()
        {
            try
            {
                DataTable datos = cReporte.FN_REPORTESPDINUEVO("3");
                if (!datos.TableName.Equals("_notificacion"))
                {
                    GridPanel_REPORTE.SetTitle("Datos: .::Reporte Actividades::.");
                    GridPanel_REPORTE.GetStore().DataSource = datos;
                    GridPanel_REPORTE.GetStore().DataBind();
                }
                else
                {
                    GridPanel_REPORTE.GetStore().RemoveAll();
                    GridPanel_REPORTE.GetStore().DataBind();
                }
            }
            catch (Exception ex)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error al consultar la información: " + ex.Message).Show();
            }
        }

        public void Export_Event(object sender, DirectEventArgs e)
        {
            try
            {
                var tipo_report = e.ExtraParams["tipo_report"];
                if (tipo_report.Equals("1"))
                {
                    DataTable dtReporte_general = cReporte.FN_REPORTESPDINUEVO("1");

                    // eliminar columnas no usadas en el reporte
                    dtReporte_general.Columns.Remove("ID_LINEA");
                    dtReporte_general.Columns.Remove("ID_PROGRAMA");
                    dtReporte_general.Columns.Remove("ID_ESTRATEGIA");

                    EXLista Lista = new EXLista();
                    List<EXEscribir> ListaEX = new List<EXEscribir>();
                    ExcelClosedXML EX = new ExcelClosedXML();

                    Lista.Add(new EXTabla { Tletra = 9, StripeRows = true, Titulo = "", ColSuma = new int[] { }, Data = dtReporte_general, Fil = 1, Col = 1 }, "Reporte_General");

                    System.IO.MemoryStream memoryStream = EX.CrearExcel(new ParametrosExcel { Data = Lista });
                    Session["DATA"] = new Dictionary<String, Object>() { { "TIPO", "MEMORYSTREAM" }, { "NOMBREARCHIVO", "REPORTE_GENERAL_AVANCE" }, { "DESCARGAINMEDIATA", "SI" }, { "ARCHIVO", memoryStream } };
                    Response.Redirect("../PaginasWeb/Descarga.aspx");
                }
                else
                {
                    X.Msg.Notify("Alerta", "No hay información. ").Show();
                }

            }
            catch (Exception ex)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error al generar el excel " + ex.Message).Show();
                throw;
            }
        }

        protected Field OnCreateFilterableField(object sender, ColumnBase column, Field defaultField)
        {
            defaultField.CustomConfig.Add(new ConfigItem("getValue", "getFiltro", ParameterMode.Raw));
            return defaultField;
        }

        #region para utilizar Mongo
        protected void VerSoporte(object sender, DirectEventArgs e)
        {
            try
            {
                //string IncaAdjunto = e.ExtraParams["MONID"].ToString();
                string IncaAdjunto = "6290ea62df146c4328ffa37c";
                MongoInfoArchivo InfArchivo = MG.DocumentoConsultarId(IncaAdjunto);

                if (InfArchivo.Archivo.Length > 0)
                {
                    if (Array.IndexOf(new String[] { "jpg", "png", "gif", "ico", "tif", "bmp", "emf", "wmf", "exif" }, InfArchivo.Extension.ToLower()) >= 0)
                    {
                        X.AddScript("App.WinVerAdjunto.setActiveItem(1);");
                        this.ImagenAdjunto.ImageUrl = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(InfArchivo.Archivo));
                        this.WinVerAdjunto.Show();
                    }
                    else if (InfArchivo.Extension.ToLower() == "pdf")
                    {
                        X.AddScript("App.WinVerAdjunto.setActiveItem(0);");
                        Session["DATA"] = new Dictionary<String, Object>() { { "NOMBREARCHIVO", "Documento.pdf" }, { "DESCARGAINMEDIATA", "NO" }, { "ARCHIVO", InfArchivo.Archivo.ToArray() } };
                        PanelContenedor1.Loader = new ComponentLoader { Url = "..\\PaginasWeb\\Descarga.aspx" };
                        PanelContenedor1.LoadContent();
                        this.WinVerAdjunto.Show();
                    }
                    else
                    {
                        Session["DATA"] = new Dictionary<String, Object>() { { "NOMBREARCHIVO", InfArchivo.NombreArchivo }, { "DESCARGAINMEDIATA", "SI" }, { "ARCHIVO", InfArchivo.Archivo.ToArray() } };

                        PanelContenedor1.Loader = new ComponentLoader { Url = "..\\PaginasWeb\\Descarga.aspx" };
                        PanelContenedor1.LoadContent();
                        this.WinVerAdjunto.Show();
                        WinVerAdjunto.Hide();
                    }


                }
                else X.Msg.Notify("Error", "El archivo adjunto no se encontró, es posible que se halla removido o cambiado de ruta");


            }
            catch (Exception)
            {
                X.Msg.Notify("Error", "El archivo adjunto no se encontró, es posible que se halla removido o cambiado de ruta");
            }
        }


        protected void AgregarArchivo_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (TArchivoSoporteMeta.HasFile)
                {
                    string ext = Path.GetExtension(TArchivoSoporteMeta.FileName);
                    string nombreArchivo = this.ObtenerNombreArchivo(TArchivoSoporteMeta.FileName);
                    string descripcion = TFDescripcionSoporteMeta.Text;
                    var datasoporte = (JObject)JsonConvert.DeserializeObject(e.ExtraParams["data"]);


                    ArchivosMongo am = new ArchivosMongo("DOC");
                    byte[] archivo = TArchivoSoporteMeta.FileBytes;
                    string fileName = "PDI" + nombreArchivo;

                    //Guardar documento adjunto en MongoDB
                    MongoRespuesta respuesta = am.guardarArchivo(archivo, ext, fileName, _pege_id);

                    if (respuesta.Tipo == MongoTipoRes.SI)
                    {


                    }
                    else
                    {
                        X.Msg.Notify("Error al guardar el archivo", "Error al guardar el archivo en la base de datos general" + respuesta.Mensaje).Show();
                    }
                }
                else
                {
                    X.Msg.Notify("Información", "No se ha seleccionado un archivo de referencia").Show();
                }
            }
            catch (Exception ex)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error al guardar archivo. Detalle: " + ex.Message).Show();
            }
        }

        public string ObtenerNombreArchivo(string name)
        {
            name = name.ToLower();
            name = name.Trim();
            return name.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u');
        }

        #endregion

        #region GENERAR PDF
        [DirectMethod(ShowMask = true, Msg = "Generando Reporte")]
        public void armaPDF()
        {

            try
            {
                string html = "<html><body> <H3> hola </H3></body> </html>";
                if (html != "No_tiene_puntos" && html != "No_tiene_jefe")
                {
                    byte[] nombrePDF = pdf.CrearDocumento(html);
                    Session["VDAnombreArchivo"] = "ReportePuntos.pdf";
                    Session["VDAopc"] = "1";
                    Session["VDAdataArchivo"] = nombrePDF;
                    Response.Redirect("../PaginasWeb/VisualizarDescargarArchivo.aspx");
                }
                else
                {

                    X.MessageBox.Alert("Notiicación", "El Docente Seleccionado No Tiene Puntos Aprobados").Show();
                }

            }
            catch (Exception e)
            {
                X.MessageBox.Alert("ERROR", "Ocurrió un error inesperado al generar el reporte de los puntos. Descripción:" + e.Message).Show();
            }


        }

        #endregion

        #region ENVIAR CORREO
        protected void BEnviarMensaje_Click(object sender, EventArgs e)
        {
            try
            {
                bool estado = this.enviarCorreoContacto("eliana pruebas", "el.cordoba@udla.edu.co", "Hola Esto es una prueba.");
                if (estado)
                {
                    X.Msg.Notify("Error", "El mensaje ha sido enviado exitosamente. En breve nos comunicaremos con usted. ").Show();
                }
                else
                {
                    X.Msg.Notify("Error", "Ha ocurrido un error al enviar el mensaje. Por favor intente nuevamente. ").Show();
                }

            }
            catch (Exception)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error al enviar el mensaje. Por favor intente nuevamente. ").Show();
            }
        }

        protected bool enviarCorreoContacto(string nombres, string correo, string mensaje)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(nombres) && !String.IsNullOrEmpty(correo) && !String.IsNullOrEmpty(mensaje))
                {
                    Email email = new Email();
                    string contenido = "";

                    contenido += "<p style='text-align:justify;'>";
                    contenido += "<b>Remitente: </b>" + nombres + "<br/><br/>";
                    contenido += "<b>Correo electrónico: </b>" + correo + "<br/><br/>";
                    contenido += "<b>Descripción: </b>" + mensaje + "<br/><br/>";
                    contenido += "</p>";

                    contenido = email.ObtenerPlantillaCorreo().Replace("{0}", contenido);

                    result = email.EnviarMail(contenido, "INFORMACIÓN EJEMPLO - Universidad de la Amazonia", "el.cordoba@udla.edu.co");
                }
            }
            catch (Exception ex)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error al enviar la información. Por favor intente nuevamente " + ex.Message).Show();

            }
            return result;
        }

        #endregion
    }
}