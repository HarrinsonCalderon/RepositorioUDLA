using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using System.Data;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public enum AlinearInfo
    {
        ArribaDerecha = AnchorPoint.TopRight,
        ArribaCentro = AnchorPoint.Top,
        ArribaIzquierda = AnchorPoint.TopLeft,
        Centro = AnchorPoint.Center,
        DerechaArriba = AnchorPoint.RightTop,
        DerechaCentro = AnchorPoint.Right,
        DerechaAbajo = AnchorPoint.RightBottom,
        IzquierdaArriba = AnchorPoint.LeftTop,
        IzquierdaCentro = AnchorPoint.Left,
        IzquierdaAbajo = AnchorPoint.LeftBottom,
        AbajoDerecha = AnchorPoint.BottomRight,
        AbajoCentro = AnchorPoint.Bottom,
        AbajoIzquierda = AnchorPoint.BottomLeft
    }

    public class Mensajes
    {
        //Mensajes genericos
        public static void NotificacionGenerico(string title, string mensaje, UI tema, Icon icono, AnchorPoint alineacion = (AnchorPoint)AlinearInfo.AbajoDerecha)
        {
            X.Msg.Info(new InfoPanel
            {
                Title = title,
                Icon = icono,
                BodyStyle = "padding:10px",
                UI = tema,
                HideDelay = 5000,
                Html = mensaje,
                Alignment = alineacion
            }
                ).Show();
        }

        public static void MensajeGeneralExcepcion()
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.QUESTION,
                Message = "Lo sentimos no fue posible procesar la solicitud, ocurrió un error de tipo excepción, inténtelo más tarde.",
                Title = "Notificación",
                Buttons = Ext.Net.MessageBox.Button.OK
            }).Show();
        }
        public static void MensajeGeneralExcepcion(String MensajeError)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.QUESTION,
                Message = "Lo sentimos no fue posible procesar la solicitud, ocurrió un error de tipo excepción, inténtelo más tarde.[ " + MensajeError + " ]",
                Title = "Notificación",
                Buttons = Ext.Net.MessageBox.Button.OK
            }).Show();
        }
        public static void MensajeGeneral(String Mensaje)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.INFO,
                Message = Mensaje,
                Title = "Notificación",
                Buttons = Ext.Net.MessageBox.Button.OK
                //MessageBoxButtonsConfig = new MessageBoxButtonsConfig { Yes = new MessageBoxButtonConfig { Handler="Ext.Msg.alert('dd','d');"} }
            }).Show();
        }
        public static void MensajeGeneral(String Mensaje, MessageBox.Icon Icono)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Icono,
                Message = Mensaje,
                Title = "Notificación",
                Buttons = Ext.Net.MessageBox.Button.OK
            }).Show();
        }
        public static void MensajeGeneral(String Titulo, String Mensaje)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.INFO,
                Message = Mensaje,
                Title = Titulo,
                Buttons = Ext.Net.MessageBox.Button.OK
            }).Show();
        }
        public static void MensajeGeneralSinBotones(String Mensaje)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.INFO,
                Message = Mensaje,
                Title = "Notificación",
                Closable = false
            }).Show();
        }
        public static void MensajeGeneral(String Titulo, String Mensaje, MessageBox.Icon Icono)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Icono,
                Message = Mensaje,
                Title = Titulo,
                Buttons = Ext.Net.MessageBox.Button.OK
            }).Show();
        }
        public static void MensajeGeneral(String Titulo, String Mensaje, MessageBox.Button Boton)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.INFO,
                Message = Mensaje,
                Title = Titulo,
                Buttons = Boton
            }).Show();
        }
        public static void MensajeGeneral(String Titulo, String Mensaje, MessageBox.Icon Icono, MessageBox.Button Boton)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Icono,
                Message = Mensaje,
                Title = Titulo,
                Buttons = Boton
            }).Show();
        }
        public static void MensajeGeneralAccionOk(String Mensaje, String HandlerOk)
        {
            X.MessageBox.Configure(new MessageBoxConfig
            {
                Icon = Ext.Net.MessageBox.Icon.INFO,
                Message = Mensaje,
                Title = "Notificación",
                Buttons = Ext.Net.MessageBox.Button.OK,
                MessageBoxButtonsConfig = new MessageBoxButtonsConfig { Ok = new MessageBoxButtonConfig { Handler = HandlerOk, Text = "Ok" } }
            }).Show();
        }

        public static void MensajeConfirmacion(String Mensaje, String HandlerSI)
        {
            X.Msg.Confirm("Confirmación", Mensaje, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = HandlerSI, Text = "Si" },
                No = new MessageBoxButtonConfig { /*Handler = "#{DirectMethod}.DoNo();",*/ Text = "No" }
            }).Show();
        }
        public static void MensajeConfirmacion(String Titulo, String Mensaje, String HandlerSI)
        {
            X.Msg.Confirm(Titulo, Mensaje, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = HandlerSI, Text = "Si" },
                No = new MessageBoxButtonConfig { /*Handler = "#{DirectMethod}.DoNo();",*/ Text = "No" }
            }).Show();
        }
        public static void MensajeConfirmacion(String Titulo, String Mensaje, String HandlerSI, String HandlerNO)
        {
            X.Msg.Confirm(Titulo, Mensaje, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = HandlerSI, Text = "Si" },
                No = new MessageBoxButtonConfig { Handler = HandlerNO, Text = "No" }
            }).Show();
        }



        //Mostrar mensaje desde la base de datos
        public static bool NotificacionBD(DataTable _answer)
        {
            var _row = _answer.Rows[0];

            switch (_row["_TITULO"].ToString())
            {
                case "Error":
                    Mensajes.NotificacionGenerico(_row["_TIPO"].ToString(), _row["_MENSAJE"].ToString(), Ext.Net.UI.Danger, Ext.Net.Icon.Cross);
                    return false;
                case "errorPin":
                    Mensajes.NotificacionGenerico(_row["_TIPO"].ToString(), _row["_MENSAJE"].ToString(), Ext.Net.UI.Danger, Ext.Net.Icon.Cross);
                    return false;
                case "Error en el procedimiento":
                    Mensajes.NotificacionGenerico(_row["_TIPO"].ToString(), _row["_MENSAJE"].ToString(), Ext.Net.UI.Danger, Ext.Net.Icon.Cross);
                    return false;
                default:
                    Mensajes.NotificacionGenerico(_row["_TIPO"].ToString(), _row["_MENSAJE"].ToString(), Ext.Net.UI.Success, Ext.Net.Icon.Tick);
                    return true;
            }
        }

        public static bool NotificacionSweetAlertBD(DataTable _answer)
        {
            var _row = _answer.Rows[0];

            switch (_row["_TITULO"].ToString())
            {
                case "Error":
                case "errorPin":
                case "Error en el procedimiento":
                    X.AddScript("Swal.fire({ icon: 'error', title: 'Oops...', text: 'Algo salió mal!' });");
                    return false;
                default:
                    X.AddScript("Swal.fire({ position: 'top-end', icon: 'success', title: 'Acción realizada con éxito', showConfirmButton: false, timer: 1800 });");
                    return true;
            }
        }


    }
}