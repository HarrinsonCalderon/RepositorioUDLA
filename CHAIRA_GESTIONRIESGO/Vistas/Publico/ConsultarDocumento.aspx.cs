using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
namespace CHAIRA_GESTIONRIESGO.Vistas.Publico
{
    public partial class ConsultarDocumento : System.Web.UI.Page
    {
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
            Node treeNode = new Node();

            Node node1 = new Node();
            Node node2 = new Node();
            Node node3 = new Node();

            treeNode.NodeID = "1";
            treeNode.Text = "Directorio 1";
            treeNode.Expanded = true;
            treeNode.Qtip = "TamaÑo 4kb, Tipo Directorio";
            treeNode.Icon = Ext.Net.Icon.Folder;
            treeNode.Expanded = true;
            //columnas inicio
            //ConfigItem auxConfig1 = new ConfigItem();
            //auxConfig1.Name = "Nombre";
            //auxConfig1.Value = "Directorio 1";

            //ConfigItem auxConfig2 = new ConfigItem();
            //auxConfig2.Name = "Tamaño";
            //auxConfig2.Value = "4 kb";
            //ConfigItem auxConfig3 = new ConfigItem();
            //auxConfig3.Name = "Tipo";
            //auxConfig3.Value = "Directorio";
            //conlumnas fin
            //treeNode.CustomAttributes.Add(new Ext.Net.ConfigItem() { 

            //});
            //treeNode.CustomAttributes.Add(auxConfig1);
            //treeNode.CustomAttributes.Add(auxConfig2);
            //treeNode.CustomAttributes.Add(auxConfig3);

            node1.Text = "Directorio 2";
            //node1.Expanded = true;
            node1.Icon = Icon.Folder;

            //
            //
            Node auxNode = new Node();
            auxNode.Text = "Registrar riesgo.pdf";
            auxNode.Icon = Ext.Net.Icon.PageWhiteAcrobat;
            auxNode.Leaf = true;
            auxNode.Href = "~/Vistas/GestionRiesgo/RegistrarRiesgo.aspx";
            auxNode.NodeID = "2";


            //ConfigItem auxConfig4 = new ConfigItem();
            //auxConfig4.Name = "Nombre";
            //auxConfig4.Value = "Directorio 2";
            //ConfigItem auxConfig5 = new ConfigItem();
            //auxConfig5.Name = "Tamaño";
            //auxConfig5.Value = "5 kb";
            //ConfigItem auxConfig6 = new ConfigItem();
            //auxConfig6.Name = "Tipo";
            //auxConfig6.Value = "Documento PDF";

            //auxNode.CustomAttributes.Add(auxConfig4);
            //auxNode.CustomAttributes.Add(auxConfig5);
            //auxNode.CustomAttributes.Add(auxConfig6);
            node1.Children.Add(auxNode);



            node1.Children.Add(new Ext.Net.Node()
            {
                Text = "Directorio 3",
                Icon = Ext.Net.Icon.Folder,
                Leaf = true,
                Href = "~/Vistas/GestionRiesgo/ConsultarRiesgo.aspx",
                Cls = "noEnlace",
                NodeID = "3"
            });


            treeNode.Children.Add(node1);


            node2.Text = "Directorio 4";
            node2.Expanded = false;
            node2.Icon = Icon.Folder;
            node2.Children.Add(new Ext.Net.Node()
            {
                Text = "Reporte del grado de exposición",
                Icon = Ext.Net.Icon.Folder,
                Leaf = true,
                Href = "~/Vistas/ReporteRiesgo/ReporteDelGradoDeExposicion.aspx",
                NodeID = "5"
            });
            node2.Children.Add(new Ext.Net.Node()
            {
                Text = "5",
                Icon = Ext.Net.Icon.Folder,
                Leaf = true,
                Href = "~/Vistas/ReporteRiesgo/ListarRiesgo.aspx",
                NodeID = "6"
            });
            treeNode.Children.Add(node2);


            return treeNode;
        }
    }
}