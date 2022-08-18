using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CHAIRA_GESTIONRIESGO.Controlador;
using MongoDB.Driver;
using CHAIRA_GESTIONRIESGO.Modelo.Models;
using ChairaMongo;

namespace CHAIRA_GESTIONRIESGO.Vistas.Publico
{
    public partial class ConsultarDocumento : System.Web.UI.Page
    {
        CConsultar Cc = new CConsultar();
        MongoNoSQL MG = new MongoNoSQL("DocumentosPOAI", "127.0.0.1:27017");
        protected void Page_Load(object sender, EventArgs e)
        {
            //Node root = this.CrearMenu();
            //Node root = new Node();
            //Node nodos = this.CrearMenu("1");
            //root.Children.Add(nodos);

            //TreePanel1.RootVisible = true;
           // TreePanel1.Root.Add(nodos);

            

             
        }


        protected void NodeLoad(object sender, NodeLoadEventArgs e)
        {
            string prefix = e.ExtraParams["prefix"] ?? "";
            
            if (!string.IsNullOrEmpty(e.NodeID))
            {
                List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenuPublico(e.NodeID,"1");
                for (int i = 0; i <lista.Count(); i++)
                {
                    Node asyncNode = new Node();
                    asyncNode.Text = lista[i].Nombre;
                    asyncNode.NodeID = lista[i].Id;
                    if (lista[i].Extencion == "Directorio")
                        asyncNode.Icon = Icon.Folder;
                    else { 
                        asyncNode.Icon = Ext.Net.Icon.PageWhiteAcrobat;
                        asyncNode.Href = lista[i].Mid;
                    }
                    e.Nodes.Add(asyncNode);

                }


            }
        }

        [DirectMethod(ShowMask = true, Msg = "Generando vista")]
        public void verPDF(string mid)
        {
            try
            {
                //string IncaAdjunto = e.ExtraParams["MONID"].ToString();
                string IncaAdjunto = mid;
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
         
    }
}