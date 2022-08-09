using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
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
            treeNode.Text = "Directorios";
            treeNode.Expanded = true;
            treeNode.Qtip = "TamaÑo 4kb, Tipo Directorio";
            treeNode.Icon = Ext.Net.Icon.Folder;
            treeNode.Expanded = true;


            node1.Text = "Directorio 2";
            //node1.Expanded = true;
            node1.Icon = Icon.Folder;

            //
            //
            Node auxNode1 = new Node();
            auxNode1.Text = "Gestion de documentos";
            auxNode1.Icon = Ext.Net.Icon.Folder;
            auxNode1.Leaf = true;
            auxNode1.Href = "~/Vistas/Privado/GestionarDocumento.aspx";
            auxNode1.NodeID = "2";

            Node auxNode2 = new Node();
            auxNode2.Text = "Roles y permisos";
            auxNode2.Icon = Ext.Net.Icon.Folder;
            auxNode2.Leaf = true;
            auxNode2.Href = "~/Vistas/Privado/GestionarRolPermiso.aspx";
            auxNode2.NodeID = "3";




            treeNode.Children.Add(auxNode1);
            treeNode.Children.Add(auxNode2);



            return treeNode;
        }
    }
}