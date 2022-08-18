using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHAIRA_GESTIONRIESGO.Controlador;
using CHAIRA_GESTIONRIESGO.Utilities;
using ChairaMongo;
using Ext.Net;
using UARepositorio.Modelo.Models;
namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
{
    public partial class GestionarRolPermiso : System.Web.UI.Page
    {
        CRol Cr = new CRol();
        CConsultar Cc = new CConsultar();
        string _pege_id;
        Util cUtil = new Util();

        MongoNoSQL MG = new MongoNoSQL("DocumentosPOAI", "127.0.0.1:27017");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
            
            
            //Node root = this.CrearMenu();
            //Node root = new Node();
            //Node nodos = this.CrearMenu();
            //root.Children.Add(nodos);

            //TreePanel1.RootVisible = true;
            //TreePanel1.Root.Add(nodos);

            //tienda
                 this.s_roles.DataSource = Cr.CargarRol();
                this.s_roles.DataBind();
                this.s_rolesEditar.DataSource = Cr.CargarEstadoRol( );
                
            }

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
            auxConfig2.Name = "estado";
            auxConfig2.Value = "true";

            //conlumnas fin
            treeNode.CustomAttributes.Add(auxConfig1);
            treeNode.CustomAttributes.Add(auxConfig2);


            //columnas inicio
            ConfigItem auxConfig4 = new ConfigItem();
            auxConfig4.Name = "nombre";
            auxConfig4.Value = "Directorio 2";
            ConfigItem auxConfigid5 = new ConfigItem();
            auxConfigid5.Name = "id";
            auxConfigid5.Value = "88";
            ConfigItem auxConfig6 = new ConfigItem();
            auxConfig6.Name = "estado";
            auxConfig6.Value = "false";
            //conlumnas fin
            node1.Icon = Icon.Folder;
            node1.CustomAttributes.Add(auxConfig4);
            node1.CustomAttributes.Add(auxConfigid5);
            node1.CustomAttributes.Add(auxConfig6);
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


            auxNode.CustomAttributes.Add(auxConfig7);
            auxNode.CustomAttributes.Add(auxConfig8);


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
        protected void NodeLoad(object sender, NodeLoadEventArgs e)
        {
            string prefix = e.ExtraParams["prefix"] ?? "";
            string idrol ="";
            if (this.c_roles.SelectedItem.Value != null)
            {
                idrol = this.c_roles.SelectedItem.Value;
            }
            
            if (!string.IsNullOrEmpty(e.NodeID) && idrol!="")
            {
                string idp = e.NodeID;
                if (Session["IdR"].ToString()=="1") {
                     idp = "1";
                     this.tienda = null;
                    this.TreePanel1 = null;
                }
                List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenuCambioRol(idp,idrol);
                if (this.TreePanel1 != null) { 
                //this.TreePanel1.Refresh();
                
                }
                for (int i = 0; i < lista.Count(); i++)
                {
                    Node asyncNode = new Node();
                    asyncNode.Text = lista[i].Nombre;
                    asyncNode.NodeID = lista[i].Id;
                    if (lista[i].Extencion == "Directorio")
                        asyncNode.Icon = Icon.Folder;
                    else
                    {
                        asyncNode.Icon = Ext.Net.Icon.PageWhiteAcrobat;
                        asyncNode.Href = lista[i].Mid;
                    }
                    //custom
                    ConfigItem auxConfig1 = new ConfigItem();
                    auxConfig1.Name = "nombre";
                    auxConfig1.Value = lista[i].Nombre; ;
                    ConfigItem auxConfig2 = new ConfigItem();
                     
                    auxConfig2.Name = "estado";
                    if(lista[i].privilegio==1)
                    auxConfig2.Value = "true";
                    else
                     auxConfig2.Value = "false";

                    asyncNode.CustomAttributes.Add(auxConfig1);
                    asyncNode.CustomAttributes.Add(auxConfig2);
                    

                    e.Nodes.Add(asyncNode);

                }
                Session["IdR"] = "2";

            }
        }
        //------------INICIO Guardar rol en la bd
        protected void AgregarRol(object sender, EventArgs e)
        {
            this.t_nombreRol.Text = "";
            this.w_crearRol.Hidden = false;

        }
        protected void GuardarRol(object sender, EventArgs e)
        {
            string nombreRol = this.t_nombreRol.Text;
            //seguir bd
            if (nombreRol.Trim()!="") {
                try {
                    Cr.AgregarRol(nombreRol);
                    this.s_roles.DataSource = Cr.CargarRol();
                    this.s_roles.DataBind();
                    X.Msg.Notify("Mensaje", "Proceso completado");
                }
                catch (Exception ea) {
                    X.Msg.Notify("Error", "Ha ocurrido un error");
                }
            }


            this.t_nombreRol.Text = "";
            this.w_crearRol.Hidden = true;

        }
        
        protected void actualiazarMenu(object sender, DirectEventArgs e) {
            Session["IdR"] = "1";
            this.tienda.Reload();
        }
        //Cancelar formulario guardar rol en la bd
        protected void CancelarRol(object sender, EventArgs e)
        {
            this.t_nombreRol.Text = "";
            this.w_crearRol.Hidden = true;

        }
        //------------FIN Guardar rol en la bd
        //------------INICIO EDITAR rol en la bd
        protected void EditarRol(object sender, EventArgs e)
        {
           
            //seleccionar combo en editar rol
            //NOTA: traer los estados de los roles para poder asignarselo en el combo
            if (this.c_roles.SelectedItem.Value!=null) {
            this.w_EditarRol.Hidden = false;
            this.t_editarNombreRol.Text = this.c_roles.SelectedItem.Text;
            this.c_estadoRoles.SelectedItems.Clear();
            this.c_estadoRoles.SelectedItems.Add(new Ext.Net.ListItem(Cr.CargarEstadoRolId(this.c_roles.SelectedItem.Value).Value));
            this.c_estadoRoles.UpdateSelectedItems();
            }
            //
            Session["idRolEditar"] = this.c_roles.SelectedItem.Value;
            //this.t_nombreRol.Text = "";
            //this.w_crearRol.Hidden = false;

        }
        protected void GuardarModificacionRol(object sender, EventArgs e)
        {
            string nombreRol = this.t_editarNombreRol.Text;
            var idEstadoRol = this.c_estadoRoles.Value;

            //seguir bd actualizar el nombre y el estado del rol



            this.t_editarNombreRol.Text = "";
            this.c_estadoRoles.SelectedItems.Clear();
            this.c_estadoRoles.UpdateSelectedItems();
            this.w_EditarRol.Hidden = true;
        }

        //Cancelar formulario modificiar rol en la bd
        protected void CancelarModificacionRol(object sender, EventArgs e)
        {
            this.t_editarNombreRol.Text = "";
            this.c_estadoRoles.SelectedItems.Clear();
            this.c_estadoRoles.UpdateSelectedItems();
            this.w_EditarRol.Hidden = true;

        }
        protected void metodo()
        {
            Console.Write("1");
        }
        [DirectMethod]
        public void CambiarRol(string nodeid)
        {
            var idrol = this.c_roles.SelectedItem.Value;
            if (idrol != null && nodeid.Trim()!="")
            {
                try {
                    Cr.CambiarEstado(nodeid,idrol);
                    this.tienda.Reload();
                X.Msg.Notify("Mensaje", "Proceso completado");
                }
                catch (Exception ea) {
                X.Msg.Notify("Error", "Ha ocurrido un error");
                }
            }
            else {
                X.Msg.Notify("Error", "Ha ocurrido un error");
            }
            


        }
            //------------FIN EDITAR rol en la bd

        }

}