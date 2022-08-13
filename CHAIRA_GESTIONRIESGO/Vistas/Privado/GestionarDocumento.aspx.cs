using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHAIRA_GESTIONRIESGO.Controlador;
using CHAIRA_GESTIONRIESGO.Modelo.Models;
using CHAIRA_GESTIONRIESGO.Utilities;
using ChairaMongo;
using Ext.Net;
namespace CHAIRA_GESTIONRIESGO.Vistas.Privado
{
    public partial class GestionarDocumento : System.Web.UI.Page
    {
        CConsultar Cc = new CConsultar();
        string _pege_id;
        Util cUtil = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            _pege_id = cUtil.GetPege();
            //Node root = this.CrearMenu();
            //Node root = new Node();
            //root.AllowDrag = false;
            //Node nodos = this.CrearMenu();
            //root.Children.Add(nodos);

            //TreePanel1.RootVisible = true;
            //TreePanel1.Root.Add(nodos);

            if (!IsPostBack) {
                this.s_comboEstado.DataSource=Cc.CargarComboEstado();
                this.s_comboEstado.DataBind();
            }



        }

        protected void NodeLoad(object sender, NodeLoadEventArgs e)
        {
            string prefix = e.ExtraParams["prefix"] ?? "";

            if (!string.IsNullOrEmpty(e.NodeID))
            {
                List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenu(e.NodeID);
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
                    auxConfig2.Name = "tamaño";
                    auxConfig2.Value = "4 kb";
                    ConfigItem auxConfig3 = new ConfigItem();
                    auxConfig3.Name = "tipo";
                    auxConfig3.Value = lista[i].Extencion;
                    ConfigItem auxConfigid = new ConfigItem();
                    auxConfigid.Name = "id";
                    auxConfigid.Value = lista[i].Id;
                    asyncNode.CustomAttributes.Add(auxConfig1);
                    asyncNode.CustomAttributes.Add(auxConfig2);
                    asyncNode.CustomAttributes.Add(auxConfig3);
                    asyncNode.CustomAttributes.Add(auxConfigid);

                    e.Nodes.Add(asyncNode);

                }


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
            Session["Padre"] = nodeid;
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

        #region gestion de archivos
        protected void AgregarArchivo_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (TArchivoSoporteMeta.HasFile)
                {
                    string ext = Path.GetExtension(TArchivoSoporteMeta.FileName);
                    string nombreArchivo = this.ObtenerNombreArchivo(TArchivoSoporteMeta.FileName.Substring(0,TArchivoSoporteMeta.FileName.LastIndexOf(".")));
                    //string descripcion = TFDescripcionSoporteMeta.Text;
                    //var datasoporte = (JObject)JsonConvert.DeserializeObject(e.ExtraParams["data"]);
                    //var datasoporte = e.ExtraParams["data"].ToString();


                    ArchivosMongo am = new ArchivosMongo("DOC");
                    byte[] archivo = TArchivoSoporteMeta.FileBytes;
                    string fileName = nombreArchivo;

                    //Guardar documento adjunto en MongoDB
                    MongoRespuesta respuesta = am.guardarArchivo(archivo, ext, fileName, _pege_id);
                    this.tienda.Reload();
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
                if (Session["Padre"]!=null) {
                    try {
                        MongoInfoArchivo2 registroMongo = Cc.CargarUltimoDocumento();
                        Cc.GuardarMenu(registroMongo, Session["Padre"].ToString());
                        //Session["Padre"] = null;
                        this.TArchivoSoporteMeta.Clear();
                        X.Msg.Notify("Mensaje", "Proceso completado");

                    }
                    catch (Exception ea) {
                        X.Msg.Notify("Error", "Ha ocurrido un error");
                    }
                    
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
        protected void AgregarCarpeta_Click(object sender, EventArgs e)
        {
            string n = t_NombreCarpeta.Text.ToString();
            
            if (Session["Padre"] != null && n.Trim() != "")
            {
                try {
                    string p = Session["Padre"].ToString();
                    MongoInfoArchivo2 registroMongo = Cc.CargarUltimoDocumento();
                    Cc.GuardarCarpeta(n, Session["Padre"].ToString());
                    //Session["Padre"] = null;
                    this.t_NombreCarpeta.Text = "";
                    this.tienda.Reload();
                    X.Msg.Notify("Mensaje", "Proceso completado");
                }
                catch (Exception ea) {
                    X.Msg.Notify("Error", "Ha ocurrido un error");
                }
              
                
            }
            else {
                X.Msg.Notify("Error", "Todos los datos son obligatorios");
            }


        }
        
       protected void CambiarNombreCarpeta_Click(object sender, EventArgs e) {
            try {
                string nombre = this.t_nombreCarpetaw.Text.ToString().Trim();
                if (nombre != "" && Session["Padre"] != null)
                {
                    Cc.ActualizarNombreCarpeta(nombre, Session["Padre"].ToString());
                    this.tienda.Reload();
                    this.t_nombreCarpetaw.Text = "";
                    X.Msg.Notify("Mensaje", "Proceso completado");
                }
                else {
                    X.Msg.Notify("Error", "Todos los datos son obligatorios");
                }

            }
            catch (Exception ea) {
                X.Msg.Notify("Error", "Ha ocurrido un error");
            }
        }
        #endregion
    }
}