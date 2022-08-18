using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Ext.Net;
using System;
namespace CHAIRA_GESTIONRIESGO.Conexion
{
    public static class NotificacionCOD
    {
        public static DataTable notificar(DataTable notificacion)
        {
            for (int i = 0; i < notificacion.Rows.Count; i++)
            {
                string tipo = notificacion.Rows[i]["_TIPO"].ToString();
                string titulo = notificacion.Rows[i]["_TITULO"].ToString();
                string mensaje = notificacion.Rows[i]["_MENSAJE"].ToString();
                Boolean pinned = tipo.Contains("Pin");
                System.Drawing.Font fuenteMensaje = new System.Drawing.Font("Arial", 11);
                System.Drawing.Size tamanoTexto = System.Windows.Forms.TextRenderer.MeasureText(mensaje, fuenteMensaje);
                mensaje = mensaje.Replace("\n", "</br>");
                int anchoNotificacion = 320;
                int altoNotificacion = 170;
                double renglones = (tamanoTexto.Height / 21) + (tamanoTexto.Width / anchoNotificacion);
                if (mensaje.Length > 0)
                {
                    if (0 <= tamanoTexto.Width && tamanoTexto.Width <= 300)
                    {
                        anchoNotificacion = 320;
                        altoNotificacion = 170;
                    }
                    else
                        if (300 <= tamanoTexto.Width && tamanoTexto.Width <= 400)
                    {
                        anchoNotificacion = tamanoTexto.Width + 20;
                        renglones = (tamanoTexto.Height / 16) + (tamanoTexto.Width / anchoNotificacion);
                        altoNotificacion = (((int)Math.Ceiling(renglones)) * 20) + 60;
                        //altoNotificacion = tamanoTexto.Height+20;
                    }
                    else
                    {
                        anchoNotificacion = 420;
                        renglones = (tamanoTexto.Height / 16) + (tamanoTexto.Width / anchoNotificacion);
                        altoNotificacion = (((int)Math.Ceiling(renglones)) * 20) + 60;
                        //altoNotificacion = ((tamanoTexto.Width/anchoNotificacion)*tamanoTexto.Height)+20;
                    }
                }
                if (pinned)
                {
                    tipo = tipo.Remove(tipo.IndexOf("Pin"));
                }
                if (tipo.Contains("ORA-20333: "))
                {
                    tipo = tipo.Split(new Char[] { ' ' })[1];
                }

                switch (tipo)
                {
                    case "notificacion":
                        Notification.Show(new NotificationConfig
                        {
                            Title = titulo,
                            Icon = Icon.Information,
                            ShowPin = true,
                            Pinned = pinned,
                            Height = altoNotificacion,
                            Width = anchoNotificacion,
                            AutoScroll = true,
                            BodyStyle = " text-align: left;padding:10px;",
                            Html = mensaje
                        });
                        break;
                    case "error":
                        Notification.Show(new NotificationConfig
                        {
                            Title = titulo,
                            Icon = Icon.Decline,
                            ShowPin = true,
                            Pinned = pinned,
                            Height = altoNotificacion + 30,
                            Width = anchoNotificacion,
                            AutoScroll = true,
                            BodyStyle = "color: red; text-align: left;padding:10px;",
                            Html = mensaje + "<div style='text-align: right; padding: 10px 10px 0px  10px; color: gray; font-style: italic;'> " + DateTime.Now.ToString("MMMM dd, yyyy (hh:mm:ss tt)") + "</div>"
                        });
                        break;
                    case "advertencia":
                        Notification.Show(new NotificationConfig
                        {
                            Title = titulo,
                            Icon = Icon.Error,
                            ShowPin = true,
                            Pinned = pinned,
                            Height = altoNotificacion,
                            Width = anchoNotificacion,
                            AutoScroll = true,
                            BodyStyle = "text-align: left;padding:10px;",
                            Html = mensaje
                        });
                        break;
                }
            }
            return notificacion;
        }

        public static DataTable notificar(string _tipo, string _titulo, string _mensaje)
        {
            DataTable datos = new DataTable();
            datos.TableName = "_notificacion";
            datos.Columns.Add("_TIPO", typeof(string));                       // Se crean las columnas del DataTable
            datos.Columns.Add("_TITULO", typeof(string));
            datos.Columns.Add("_MENSAJE", typeof(string));
            datos.Rows.Add(                                                     // Se adiciona la fila con el mensaje  
                _tipo,     //tipo
                _titulo,     //titulo
                _mensaje);    //mensaje

            return notificar(datos);
        }
    }
}