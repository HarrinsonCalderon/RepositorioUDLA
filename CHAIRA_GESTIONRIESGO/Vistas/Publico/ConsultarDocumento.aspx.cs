using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CHAIRA_GESTIONRIESGO.Controlador;
namespace CHAIRA_GESTIONRIESGO.Vistas.Publico
{
    public partial class ConsultarDocumento : System.Web.UI.Page
    {
        CConsultar Cc = new CConsultar();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Node root = this.CrearMenu();
            Node root = new Node();
            Node nodos = this.CrearMenu();
            root.Children.Add(nodos);

            TreePanel1.RootVisible = true;
            TreePanel1.Root.Add(nodos);

            

             
        }
        




        private Node CrearMenu()
        {
            List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenu();
            Node treeNode = new Node();

            Node node1 = new Node();
            Node node2 = new Node();
            Node node3 = new Node();

            treeNode.NodeID = lista[0].Id;
            treeNode.Text = lista[0].Nombre;
            treeNode.Expanded = true;
            //treeNode.Qtip = "TamaÑo 4kb, Tipo Directorio";
            treeNode.Icon = Ext.Net.Icon.Folder;
            treeNode.Expanded = true;
            List<string> vis = new List<string>();

            for (int i=1;i<lista.Count();i++) {

                Node nodex = new Node();
                nodex.NodeID = lista[i].Id;
                nodex.Text = lista[i].Nombre;
                nodex.Expanded = true;
                nodex.NodeID = lista[i].Id;
                nodex.Icon = Icon.Folder;
                vis.Add(lista[i].Id);

                treeNode.Children.Add(nodex);
            }

            node1.Text = "Directorio 2";
            node1.Icon = Icon.Folder;

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


            return treeNode;
        }
    }
}