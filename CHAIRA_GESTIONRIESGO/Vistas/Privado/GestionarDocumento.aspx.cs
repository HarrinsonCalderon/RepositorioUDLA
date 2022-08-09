using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
{
    public partial class GestionarDocumento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Node root = this.CrearMenu();
            Node root = new Node();
            root.AllowDrag = false;
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
            //treeNode.Text = "Directorio 1";
            treeNode.Expanded = true;
            treeNode.Qtip = "Tooltip1";
            treeNode.Icon = Ext.Net.Icon.Folder;
            treeNode.Expanded = true;
            //columnas inicio
            ConfigItem auxConfig1 = new ConfigItem();
            auxConfig1.Name = "nombre";
            auxConfig1.Value = "Directorio 1";

            ConfigItem auxConfig2 = new ConfigItem();
            auxConfig2.Name = "tamaño";
            auxConfig2.Value = "4 kb";
            ConfigItem auxConfig3 = new ConfigItem();
            auxConfig3.Name = "tipo";
            auxConfig3.Value = "Carpeta";
            ConfigItem auxConfigid = new ConfigItem();
            auxConfigid.Name = "id";
            auxConfigid.Value = "66";
            //conlumnas fin

            treeNode.CustomAttributes.Add(auxConfig1);
            treeNode.CustomAttributes.Add(auxConfig2);
            treeNode.CustomAttributes.Add(auxConfig3);
            treeNode.CustomAttributes.Add(auxConfigid);

            //columnas inicio
            ConfigItem auxConfig4 = new ConfigItem();
            auxConfig4.Name = "nombre";
            auxConfig4.Value = "Directorio 2";

            ConfigItem auxConfig5 = new ConfigItem();
            auxConfig5.Name = "tamaño";
            auxConfig5.Value = "6 kb";
            ConfigItem auxConfig6 = new ConfigItem();
            auxConfig6.Name = "tipo";
            auxConfig6.Value = "Carpeta";
            ConfigItem auxConfigid2 = new ConfigItem();
            auxConfigid2.Name = "id";
            auxConfigid2.Value = "88";
            //conlumnas fin
            node1.Icon = Icon.Folder;
            node1.CustomAttributes.Add(auxConfig4);
            node1.CustomAttributes.Add(auxConfig5);
            node1.CustomAttributes.Add(auxConfig6);
            node1.CustomAttributes.Add(auxConfigid2);
            //
            Node auxNode = new Node();
            auxNode.Text = "Registrar riesgo.pdf";
            auxNode.Icon = Ext.Net.Icon.PageWhiteAcrobat;
            auxNode.Leaf = true;
            auxNode.Href = "~/Vistas/GestionRiesgo/RegistrarRiesgo.aspx";
            auxNode.NodeID = "2";


            ConfigItem auxConfig7 = new ConfigItem();
            auxConfig7.Name = "nombre";
            auxConfig7.Value = "Registrar riesgo.pdf";
            ConfigItem auxConfig8 = new ConfigItem();
            auxConfig8.Name = "tamaño";
            auxConfig8.Value = "6 kb";
            ConfigItem auxConfig9 = new ConfigItem();
            auxConfig9.Name = "tipo";
            auxConfig9.Value = "Documento PDF";

            auxNode.CustomAttributes.Add(auxConfig7);
            auxNode.CustomAttributes.Add(auxConfig8);
            auxNode.CustomAttributes.Add(auxConfig9);

            // hijo
            Node auxNode2 = new Node();
            auxNode2.Text = "Registrar riesgo.pdf";
            auxNode2.Icon = Ext.Net.Icon.PageWhiteAcrobat;
            auxNode2.Leaf = true;
            auxNode2.Href = "~/Vistas/GestionRiesgo/RegistrarRiesgo.aspx";
            auxNode2.NodeID = "2";





            // hijo 
            auxNode.Children.Add(auxNode2);
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
            node2.Children.Add(node1);

            treeNode.Children.Add(node1);


            return treeNode;
        }


        //metodo elegir ventana
        [DirectMethod]
        public void cargarEvidencia(Object a)
        {
            Console.Write("si");

        }
        [DirectMethod]
        public void ActivarVentana(string nodeid, string op)
        {
            if (op == "1")
            {
                this.w_ventana.Hidden = false;
            }
            else
            {
                this.w_gestionarArchivo.Hidden = false;
            }
            //Console.Write("si");

        }
    }
}