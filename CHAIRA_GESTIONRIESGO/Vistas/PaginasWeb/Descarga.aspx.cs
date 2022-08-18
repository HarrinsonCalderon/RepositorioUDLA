using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHAIRA_GESTIONRIESGO.Vistas.PaginasWeb
{
    public partial class Descarga : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["DATA"] != null)
                    {
                        //DescargaArchivo DA = (DescargaArchivo)Session["DATA"]; 
                        Dictionary<string, object> DA = (Dictionary<string, object>)Session["DATA"];
                        if (!DA.ContainsKey("TIPO"))
                        {
                            DA.Add("TIPO", "PDF");
                        }
                        switch (DA["TIPO"].ToString())
                        {
                            case "PDF":
                                #region DESCARGA DE ARCHIVO EN PDF
                                byte[] FileBuffer = (byte[])DA["ARCHIVO"];
                                if (FileBuffer != null)
                                {
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/pdf";
                                    if (DA["DESCARGAINMEDIATA"].ToString() == "SI")
                                        Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", String.IsNullOrEmpty(DA["NOMBREARCHIVO"].ToString()) ? "Archivo.pdf" : DA["NOMBREARCHIVO"].ToString()));//Descarga directa del archivo
                                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.BinaryWrite(FileBuffer);
                                    Response.End();
                                }
                                #endregion
                                break;
                            case "MEMORYSTREAM":
                                #region DESCARGA DE ARCHIVO EN EXCEL
                                MemoryStream memorystream = (MemoryStream)DA["ARCHIVO"];

                                Response.Clear();
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", (DA.ContainsKey("NOMBREARCHIVO") ? DA["NOMBREARCHIVO"] : "Reporte")));
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                memorystream.WriteTo(Response.OutputStream);
                                Response.End();
                                #endregion

                                break;
                        }
                        Session.Remove("DATA");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}