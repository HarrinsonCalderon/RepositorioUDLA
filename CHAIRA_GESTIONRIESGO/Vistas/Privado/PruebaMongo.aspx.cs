using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHAIRA_GESTIONRIESGO.Utilities;
using ChairaMongo;
using Ext.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CHAIRA_GESTIONRIESGO.Modelo.Models;
namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
{
    public partial class PruebaMongo : System.Web.UI.Page
    {
        string _pege_id;
        Util cUtil = new Util();
        
        MongoNoSQL MG = new MongoNoSQL("fs.files", "mongodb://localhost:27017");
        protected void Page_Load(object sender, EventArgs e)
        {
            _pege_id = cUtil.GetPege();
        }


        protected void VerSoporte2(object sender, DirectEventArgs e)
        {
            string IncaAdjunto = "62ee849902809544acbb9032";
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("DocumentosPOAI");
            var collection1 = database.GetCollection<MongoInfoArchivo2>("fs.files");
            var collection2 = database.GetCollection<MongoInfoArchivo2>("fs.chunks");


            List<MongoInfoArchivo2> result1 = (from d in collection1.AsQueryable<MongoInfoArchivo2>()

                                               select new MongoInfoArchivo2
                                               {
                                                   length = d.length,
                                                   Id = d.Id,
                                                   filename = d.filename,
                                               }
                         ).ToList();
            List<MongoInfoArchivo2> result2 = (from d in collection2.AsQueryable<MongoInfoArchivo2>()

                                               select new MongoInfoArchivo2
                                               {
                                                   length = d.length,
                                                   Id = d.Id,
                                                   data = d.data,
                                               }
                         ).ToList();
            MongoInfoArchivo InfArchivo = new MongoInfoArchivo();
            foreach (var it in result1)
            {
                if (it.Id.ToString() == IncaAdjunto)
                {
                    InfArchivo.Id = IncaAdjunto.ToString();
                    InfArchivo.NombreArchivo = it.filename;
                    break;
                }
            }
            foreach (var it in result2)
            {
                if (it.Id.ToString() == IncaAdjunto)
                {

                    InfArchivo.Archivo = it.data;
                    break;
                }
            }
            InfArchivo.Extension = InfArchivo.NombreArchivo.Substring(InfArchivo.NombreArchivo.IndexOf(".") + 1);
        }
        protected void VerSoporteEEE(object sender, DirectEventArgs e)
        {
            try
            {
                //string IncaAdjunto = e.ExtraParams["MONID"].ToString();
                //string IncaAdjunto = "62ee8304028095420cf741da";
                // H
                string IncaAdjunto = "62f7daf60280952d2092179d";
                var cliente = new MongoClient("mongodb://localhost:27017");
                var database = cliente.GetDatabase("DocumentosPOAI");
                var collection1 = database.GetCollection<MongoInfoArchivo2>("fs.files");
                var collection2 = database.GetCollection<MongoInfoArchivo2>("fs.chunks");


                List<MongoInfoArchivo2> result1 = (from d in collection1.AsQueryable<MongoInfoArchivo2>()

                                                   select new MongoInfoArchivo2
                                                   {
                                                       length = d.length,
                                                       Id = d.Id,
                                                       filename = d.filename,
                                                   }
                             ).ToList();
                List<MongoInfoArchivo2> result2 = (from d in collection2.AsQueryable<MongoInfoArchivo2>()

                                                   select new MongoInfoArchivo2
                                                   {
                                                       length = d.length,
                                                       Id = d.Id,
                                                       data = d.data,
                                                       files_id = d.files_id
                                                   }
                             ).ToList();
                MongoInfoArchivo InfArchivo = new MongoInfoArchivo();

                foreach (var it in result1)
                {
                    if (it.Id.ToString() == IncaAdjunto)
                    {
                        InfArchivo.Id = IncaAdjunto.ToString();
                        InfArchivo.NombreArchivo = it.filename;
                        break;
                    }
                }
                foreach (var it2 in result2)
                {
                    if (it2.files_id.ToString() == IncaAdjunto)
                    {
                        //int ii = 0;
                        // foreach(var i in it2.data) { 

                        InfArchivo.Archivo = it2.data;
                        //  ii++;
                        //}
                        break;
                    }
                }
                InfArchivo.Extension = InfArchivo.NombreArchivo.Substring(InfArchivo.NombreArchivo.IndexOf(".") + 1);
                //
                //MongoInfoArchivo InfArchivo = MG.DocumentoConsultarId(IncaAdjunto);

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
        protected void VerSoporteDTI(object sender, DirectEventArgs e)
        {
            try
            {
                ArchivosMongo am = new ArchivosMongo("DOC");
               
                //string IncaAdjunto = e.ExtraParams["MONID"].ToString();
                string IncaAdjunto = "62f7daf60280952d2092179d";
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
                    //string descripcion = TFDescripcionSoporteMeta.Text;
                    //var datasoporte = (JObject)JsonConvert.DeserializeObject(e.ExtraParams["data"]);
                    //var datasoporte = e.ExtraParams["data"].ToString();


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


    }
}