using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Ext;
using Ext.Net;
using System.Security.Permissions;
using ChairaMongo;
using CHAIRA_GESTIONRIESGO.Conexion;
using Image = System.Drawing.Image;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace CHAIRA_GESTIONRIESGO.Vistas.PaginasWeb
{
    public partial class VisualizarDescargarArchivo : System.Web.UI.Page
    {
        private Utilities.Util util = new Utilities.Util();
        readonly MongoNoSQL _mG = new MongoNoSQL(ConfigurationManager.AppSettings["SERVER_MONGO"]);
        MongoNoSQL _Mongo = new MongoNoSQL("172.16.31.19:27017");
        MongoInfoArchivo IMAGENPERFIL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params.Keys[0] == "mongoid")
                {
                    this.CargarArchivoInstructivo(Request.QueryString["mongoid"]);
                }
                if (Session["VDAnombreArchivo"] == null || Session["VDAopc"] == null || Session["VDAdataArchivo"] == null)
                    return;
                //util.GetPege();
                try
                {
                    string nombreArchivo = (Session["VDAnombreArchivo"] != null ? Session["VDAnombreArchivo"].ToString() : "");
                    nombreArchivo = nombreArchivo.Replace(" ", "_");
                    string opc = Session["VDAopc"].ToString();
                    byte[] dataArchivo = (byte[])(Session["VDAdataArchivo"]);
                    Session["VDAnombreArchivo"] = Session["VDAopc"] = Session["VDAdataArchivo"] = null;

                    Response.Clear();

                    switch (opc)
                    {
                        case "1":
                            Response.ContentType = " application/octet-stream";
                            Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + "");
                            break;
                        case "2":
                            //Response.ContentType = util.GetContentType(Path.GetExtension(nombreArchivo)); //"application/octet-stream";
                            Response.ContentType = Path.GetExtension(nombreArchivo); //"application/octet-stream";
                            Response.AppendHeader("Content-Disposition", "inline;filename=" + nombreArchivo + "");
                            break;
                    }

                    Response.BufferOutput = true;
                    Response.AddHeader("Content-Length", dataArchivo.Length.ToString());
                    Response.BinaryWrite(dataArchivo);
                    Response.End();
                }
                catch (Exception ex)
                {
                    Session["VDAnombreArchivo"] = Session["VDAopc"] = Session["VDAdataArchivo"] = null;
                }
            }

        }
        private void CargarArchivoInstructivo(string mongoid)
        {
            try
            {
                if (mongoid != "null" || !mongoid.Equals("null"))
                {
                    MongoInfoArchivo _mongoArchivo = _mG.DocumentoConsultarId("DocumentosChaira", mongoid);
                    Session["VDAnombreArchivo"] = _mongoArchivo.NombreArchivo;
                    Session["VDAdataArchivo"] = _mongoArchivo.Archivo;

                    Session["VDAopc"] = "2";

                }
                else
                {
                    this.cargarnopic();
                }
            }
            catch (Exception ex)
            {
                NotificacionCOD.notificar("error", "Error", "Ha surgido un Error" + ex.Message);
            }
        }

        public void cargarnopic()
        {
            try
            {
                MongoNoSQL MG = new MongoNoSQL("ImagenGeneral", "172.16.31.19:27017");
                IMAGENPERFIL = MG.DocumentoConsultarId("599761a7bfb1846ec8f74d5d");

                var image = (byte[])IMAGENPERFIL.Archivo;
                Response.ContentType = "image/jpeg";
                using (var ms = new System.IO.MemoryStream(image))
                {
                    using (var jpg = (Bitmap)Image.FromStream(ms))
                    {
                        jpg.Save(Response.OutputStream, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {

                NotificacionCOD.notificar("error", "Error", "Error en la conexion con Mongobd" + ex.Message);
            }

        }
    }
}