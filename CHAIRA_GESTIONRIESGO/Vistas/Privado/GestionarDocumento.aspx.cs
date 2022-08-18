using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHAIRA_GESTIONRIESGO.Controlador;
using CHAIRA_GESTIONRIESGO.Modelo.Models;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
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
       

        MongoNoSQL MG = new MongoNoSQL("DocumentosPOAI", "127.0.0.1:27017");
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
            //Tomar el rol del usuario conectado
           
            if (!string.IsNullOrEmpty(e.NodeID))
            {
                List<CHAIRA_GESTIONRIESGO.Modelo.Modelsbd.Menu> lista = Cc.CargarMenu2(e.NodeID,Session["usuarioRol"].ToString());
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
                try {
                    Combo c = Cc.CargarFormularioCarpeta((Session["Padre"].ToString()));

                    this.c_Estado.SelectedItems.Clear();
                    this.c_Estado.SelectedItems.Add(new Ext.Net.ListItem(c.Value2));
                    this.c_Estado.UpdateSelectedItems();
                    this.t_nombreCarpetaw.Text = c.Text;
                    this.w_ventana.Hidden = false;

                }
                catch (Exception ea) {
                    X.Msg.Notify("Error", "Ha ocurrido un error");
                }
              
            }
            else
            {
                this.w_gestionarArchivo.Hidden = false;
                try
                {
                    Combo c = Cc.CargarFormularioCarpeta((Session["Padre"].ToString()));
                    this.t_nombre_Archivo.Text = c.Text;
                    this.VerSoporte(c.Value3);



                }
                catch (Exception ea)
                {
                    X.Msg.Notify("Error", "Ha ocurrido un error");
                }
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
                    //MongoInfoArchivo2 registroMongo = Cc.CargarUltimoDocumento();
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
                    this.tienda.Reload();
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
        
         protected void w_GuardarCarpeta(object sender, EventArgs e)
        {
            try
            {
                string id = this.c_Estado.SelectedItem.Value;
                string nombreCarpeta = this.t_nombreCarpetaw.Text.ToString().Trim().Substring(0, t_nombreCarpetaw.Text.ToString().Trim().IndexOf(","));
                string cEstado = "";
                if (this.c_Estado.Value != null) { 
                cEstado = this.c_Estado.SelectedItem.Value.ToString();
                }  
                    
                if (id != "" && Session["Padre"] != null && nombreCarpeta!="" && cEstado!="")
                {
                    if (Cc.ActualizarNombreCarpeta(nombreCarpeta, Session["Padre"].ToString()) && Cc.ActualizarEstadoCarpeta(Session["Padre"].ToString(),cEstado) ) {
                        this.tienda.Reload();
                        this.t_nombreCarpetaw.Text = "";
                        this.tienda.Reload();
                        this.w_ventana.Hidden = true;
                        X.Msg.Notify("Mensaje", "Proceso completado");
                    }else {
                        X.Msg.Notify("Error", "Ha ocurrido un error");
                    }
                        
                   
                }
                else
                {
                    X.Msg.Notify("Error", "Todos los datos son obligatorios");
                }

            }
            catch (Exception ea)
            {
                X.Msg.Notify("Error", "Ha ocurrido un error");
            }
        }
        #endregion



        #region MOngo
        protected void VerSoporte(string mid)
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

        #endregion
    }
}