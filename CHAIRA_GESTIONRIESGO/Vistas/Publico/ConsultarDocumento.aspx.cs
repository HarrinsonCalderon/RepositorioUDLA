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
                List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenu(e.NodeID);
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
                //string IncaAdjunto = "62ee8304028095420cf741da";
                // H
                string IncaAdjunto = mid;
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
        private List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> CrearMenu(string p)
        {
            //Funciona
            List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenu(p);
            return lista;
            //Node treeNode = new Node();

            //Node node1 = new Node();
            //Node node2 = new Node();
            //Node node3 = new Node();

            //treeNode.NodeID = lista[0].Id;
            //treeNode.Text = lista[0].Nombre;
            //treeNode.Expanded = true;
            ////treeNode.Qtip = "TamaÑo 4kb, Tipo Directorio";
            //treeNode.Icon = Ext.Net.Icon.Folder;
            //treeNode.Expanded = true;
            //List<string> vis = new List<string>();

            //for (int i=1;i<lista.Count();i++) {

            //    Node nodex = new Node();
            //    nodex.NodeID = lista[i].Id;
            //    nodex.Text = lista[i].Nombre;
            //    nodex.Expanded = true;
            //    nodex.NodeID = lista[i].Id;
            //    if(lista[i].Extencion=="Directorio")
            //    nodex.Icon = Icon.Folder;
            //    else
            //    nodex.Icon = Ext.Net.Icon.PageWhiteAcrobat;
            //    vis.Add(lista[i].Id);

            //    treeNode.Children.Add(nodex);
            //}

            //node1.Text = "Directorio 2";
            //node1.Icon = Icon.Folder;
            //funciona
             


            //Node auxNode = new Node();
            //auxNode.Text = "Registrar riesgo.pdf";
            //auxNode.Icon = Ext.Net.Icon.PageWhiteAcrobat;
            //auxNode.Leaf = true;
            //auxNode.Href = "~/Vistas/GestionRiesgo/RegistrarRiesgo.aspx";
            //auxNode.NodeID = "2";

            //treeNode.Children.Add(node1);


            //node2.Text = "Directorio 4";
            //node2.Expanded = false;
            //node2.Icon = Icon.Folder;
            //node2.Children.Add(new Ext.Net.Node()
            //{
            //    Text = "Reporte del grado de exposición",
            //    Icon = Ext.Net.Icon.Folder,
            //    Leaf = true,
            //    Href = "~/Vistas/ReporteRiesgo/ReporteDelGradoDeExposicion.aspx",
            //    NodeID = "5"
            //});
            //node2.Children.Add(new Ext.Net.Node()
            //{
            //    Text = "5",
            //    Icon = Ext.Net.Icon.Folder,
            //    Leaf = true,
            //    Href = "~/Vistas/ReporteRiesgo/ListarRiesgo.aspx",
            //    NodeID = "6"
            //});
            //treeNode.Children.Add(node2);


             
        }
    }
}