using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Data;
using CHAIRA_GESTIONRIESGO.Conexion;
using CHAIRA_GESTIONRIESGO.Modelo;
using CHAIRA_GESTIONRIESGO.Controlador;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class Email
    {

        CCorreos _CCorreos = new CCorreos();

        public DataTable ConsultaCredencialesEmail()
        {
            try
            {
                CProcedimientos cod = new CProcedimientos();
                List<Parametro> para = new List<Parametro>();
                para.Add(new Parametro("CREDENCIAL", "", "CURSOR", ParameterDirection.ReturnValue));
                return cod.EjecutarSelect("CHAIRA.CREDENCIALES_EMAIL", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public string ObtenerPlantillaCorreo()
        {
            return
                "<html"
+ " <head >"
+ "     <title></title>"
+ "     <style type='text/css'>"
+ "         .style1"
+ "         {"
+ "             width: 90px;"
+ "         }"
+ "         .style2"
+ "         {"
+ "             width: 734px;"
+ "         }"
+ "         .style3"
+ "         {"
+ "             width: 90px;"
+ "             height: 67px;"
+ "         }"
+ "         .style4"
+ "         {"
+ "             width: 734px;"
+ "             height: 67px;"
+ "         }"
+ "         .style5"
+ "         {"
+ "             height: 67px;"
+ "         }"
+ "         .style6"
+ "         {"
+ "             width: 199px;"
+ "         }"
+ "         .style7"
+ "         {"
+ "             width: 90px;"
+ "             height: 427px;"
+ "         }"
+ "         .style8"
+ "         {"
+ "             width: 734px;"
+ "             height: 427px;"
+ "         }"
+ "         .style9"
+ "         {"
+ "             height: 427px;"
+ "         }"
+ "         .auto-style1 {"
+ "             height: 67px;"
+ "             width: 122px;"
+ "         }"
+ "         .auto-style2 {"
+ "             height: 389px;"
+ "             width: 122px;"
+ "         }"
+ "         .auto-style3 {"
+ "             width: 122px;"
+ "         }"
+ "         .auto-style4 {"
+ "             width: 547px;"
+ "             height: 67px;"
+ "         }"
+ "         .auto-style5 {"
+ "             width: 547px;"
+ "             height: 389px;"
+ "         }"
+ "         .auto-style6 {"
+ "             width: 547px;"
+ "         }"
+ "         .auto-style7 {"
+ "             width: 260px;"
+ "         }"
+ "         .auto-style8 {"
+ "             width: 90px;"
+ "             height: 389px;"
+ "         }"
+ "         </style>"
+ " </head>"
+ " <body>"
+ "     <form >"
+ "     <div>"
+ "     "
+ "         <table style='width:60%;'>"
+ "             <tr>"
+ "                 <td class='style3'>"
+ "                     </td>"
+ "                 <td class='auto-style4' align='center' bgcolor='White'>"
+ "                     &nbsp;<img src='https://chaira.uniamazonia.edu.co/Chaira/Resources/Images/encabezado.png'  "
+ "                         style='height: 90px; width: 600px' /></td>"
+ "                 <td class='auto-style1' bgcolor='White'>"
+ "                     </td>"
+ "             </tr>"
+ "             <tr>"
+ "                 <td class='auto-style8' bgcolor='White'>"
+ "                     </td>"
+ "                 <td class='auto-style5'>"
+ "                     <div style='margin: auto; border-style: none; border-width: thin; padding: inherit; height: auto; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-style: normal; background-color: #FFFFFF; width: 539px; font-size: small;'>"
+ "                         <br /> {0}"
+ "                     </div>"
+ "                 </td>"
+ "                 <td class='auto-style2'>"
+ "                     </td>"
+ "             </tr>"
+ "               <tr>"
+ "                   <td class='style3'></td>"
+ "                   <td class='auto-style4' align='center' bgcolor='White'><img src = 'https://chaira.uniamazonia.edu.co/Chaira/Resources/Images/encabeza.png' "
+ "                       style='height: 120px; width: 800px' /></td>"
+ "                   <td class='auto-style1' bgcolor='White'></td>"
+ "              </tr>"
+ "         </table>"
+ "     "
+ "     </div>"
+ "     </form>"
+ " </body>"
+ " </html>";
        }

        public bool EnviarMail(string contenido, string asunto, string destinatario, Attachment Adjunto = null)
        {
            try
            {
                var config = this.ConsultaCredencialesEmail();
                return config.Rows[0]["USE_SMTP"].ToString().Equals("1") ? EnviarMail_smtp(contenido, asunto, destinatario, config.Rows[0], Adjunto) : EnviarMail_BdConfig(contenido, asunto, destinatario, config.Rows[0], Adjunto);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Funcion que Envia Emails de cualquier tipo desde el email TESTER@UA.EDU.CO bajo el nombre de Tucan UniAmazonia
        /// </summary>
        /// <param name="Content">EL CUERPO DEL MENSAJE EN UN STRING CON FORMATO HTML</param>
        /// <param name="subject">ASUNTO DEL MENSAJE EN UN STRING </param>
        /// <param name="destinatario">EMAIL HACIA QUIEN VA DIRIGIDO</param>
        /// <param name="Return">MENSAJE DE CONFIRMACION PARA CADA TIPO DE CORREO EN FORMATO STRING</param>
        /// <param name="Adjunto">OBJETO TIPO NET:MAIL:ATTACHMENT DONDE VIENE LA RUTA DE UN ARCHIVO PARA SER ADJUNTADO</param>
        /// <param name="adjunto">BOOLEANO QUE DEFINE SI VIENE UN ARCHIVO ADJUNTO(true) o NO(false)</param>
        /// <returns></returns>
        private bool EnviarMail_BdConfig(string Content, string subject, string destinatario, DataRow config, Attachment Adjunto = null)
        {
            try
            {
                var smtpServer = new SmtpClient(config["MAIL_SMTP"].ToString());
                try
                {
                    var msg = new MailMessage();
                    msg.To.Add(destinatario);
                    msg.From = new MailAddress(config["MAIL_FROM"].ToString(), config["MAIL_DISPLAYNAME"].ToString(), System.Text.Encoding.UTF8);
                    msg.Subject = subject;
                    msg.Body = Content;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.IsBodyHtml = true;

                    if (Adjunto != null)
                        msg.Attachments.Add(Adjunto);

                    smtpServer.Port = Convert.ToInt32(config["MAIL_PORT"].ToString());
                    smtpServer.Credentials = new System.Net.NetworkCredential(config["MAIL_FROM"].ToString(), config["MAIL_PASSWORD"].ToString());
                    smtpServer.EnableSsl = Convert.ToBoolean(config["MAIL_SSL"].ToString());
                    smtpServer.Timeout = Convert.ToInt32(config["MAIL_TIMEOUT"].ToString());

                    smtpServer.Send(msg);
                    smtpServer.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    smtpServer.Dispose();
                    return EnviarMail_smtp(Content, subject, destinatario, config, Adjunto);
                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }

        private bool EnviarMail_smtp(string Content, string subject, string Destinatario, DataRow config, Attachment Adjunto = null)
        {
            try
            {
                //config["SMTP_PASS"].ToString()
                MailMessage msg = new MailMessage();
                msg.To.Add(Destinatario);
                msg.From = new MailAddress(config["MAIL_FROM"].ToString(), config["MAIL_DISPLAYNAME"].ToString(), System.Text.Encoding.UTF8);
                msg.Subject = subject;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = Content;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
                if (Adjunto != null)
                    msg.Attachments.Add(Adjunto);
                /*
                SmtpClient client = new SmtpClient("pod51004.outlook.com",587);
                client.UseDefaultCredentials=false;
                client.Credentials = new System.Net.NetworkCredential("no-reply@udla.edu.co", "ghothh");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                */
                SmtpClient client = new SmtpClient
                {
                    Host = config["SMTP_SERVER"].ToString(),
                    Credentials = new System.Net.NetworkCredential(config["SMTP_MAIL"].ToString(), config["SMTP_PASS"].ToString()),
                    EnableSsl = false
                };

                /*
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("no-reply@uniamazonia .edu.co", "ghothh");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;*/


                client.Send(msg);
                client.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string EnviarMailRegistroResolucionEncargo(DataRow row)
        {
            string res = "";
            string contenido = string.Format(row["MEJE"].ToString(), row["JEFE"].ToString(), row["DEPENDENCIA"].ToString(), row["CONSECUTIVO"].ToString(), row["CARGO"].ToString(), row["FUNCIONARIO"].ToString(), row["DURACION"].ToString());
            contenido = this.ObtenerPlantillaCorreo().Replace("{0}", contenido);

            res = "<br/>" + (EnviarMail(contenido, "Registro Resolución de Encargo", row["MAILJ"].ToString()) ?
                "Se notificó a " + row["JEFE"].ToString() + " mediante correo electrónico" :
                "No fue posible notificar a " + row["JEFE"].ToString() + " mediante correo electrónico");


            contenido = string.Format(row["MEFU"].ToString(), row["FUNCIONARIO"].ToString(), row["CARGO"].ToString(), row["CONSECUTIVO"].ToString(), row["JEFE"].ToString(), row["DEPENDENCIA"].ToString(), row["DURACION"].ToString());
            contenido = this.ObtenerPlantillaCorreo().Replace("{0}", contenido);

            res += "<br/>" + (EnviarMail(contenido, "Registro Resolución de Encargo", row["MAILF"].ToString()) ?
                "Se notificó a " + row["FUNCIONARIO"].ToString() + " mediante correo electrónico" :
                "No fue posible notificar a " + row["FUNCIONARIO"].ToString() + " mediante correo electrónico");

            return res;
        }

        public bool NotificacionRespuestaSolicitudResservaSala(DataRow row, string commets) //string estado
        {
            try
            {
                string res = @"<h4>Cordial saludo {0}</h4>
                    <p>Su solicitud de reserva para la sala {1} a partir de las {2} por una duración de {3} para los dias {4} al {5} a sido:  {6} </p>
                    <p><h5>Motivo: </h5>  {7} </p>
                    <p>Agradecemos su solicitud.</p>";

                string contenido = string.Format(res, row["NAME"].ToString(), row["RECU_NOMBRE"].ToString(), row["HORA"].ToString(), row["SOSA_LASTING"].ToString(), row["FECHA_INI"].ToString(), row["FECHA_FIN"].ToString(), commets, row["SOSA_APPNEG_COMMENTS"].ToString());
                contenido = this.ObtenerPlantillaCorreo().Replace("{0}", contenido);

                return EnviarMail(contenido, "INFORMACIÓN ESTADO SOLICITUD RESERVA SALA", row["EMAIL"].ToString());// row["MAIL"].ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string EnviarEmail(string Contenido, string Asunto, string MailBcc, DataTable Destinatarios, DataRow Credenciales, Attachment[] Adjuntos = null, string firma = "<b>UNIVERSIDAD DE LA AMAZONIA</b><br /> Calle 17 Diagonal 17 con Carrera 3F - Barrio Porvenir<br/>(+57) 8-4366160")
        {
            int inicio = 0, fin = 0, aux = 0;
            try
            {
                MailMessage Mensaje = new MailMessage();

                Mensaje.Subject = Asunto;

                Mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                Mensaje.Body = System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath + @"Recursos\formato.txt").Replace("{CONTENIDO}", Contenido).Replace("{FIRMA}", firma);
                Mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                Mensaje.IsBodyHtml = true;
                Mensaje.Bcc.Add(MailBcc);

                if (Adjuntos != null)
                    for (int i = 0; i < Adjuntos.Length; i++) Mensaje.Attachments.Add(Adjuntos[i]);

                Mensaje.From = new MailAddress(Credenciales["MAIL_FROM"].ToString(), Credenciales["DISPLAYNAME"].ToString(), System.Text.Encoding.UTF8);

                SmtpClient client = new SmtpClient();

                client.Port = Convert.ToInt32(Credenciales["PORT"].ToString());
                client.Host = Credenciales["SERVER"].ToString();
                client.Credentials = new System.Net.NetworkCredential(Credenciales["MAIL"].ToString(), Credenciales["PASS"].ToString());
                if (Credenciales.Table.Columns.Contains("SSL")) client.EnableSsl = Convert.ToBoolean(Credenciales["SSL"].ToString());
                if (Credenciales.Table.Columns.Contains("MAIL_TIMEOUT")) client.Timeout = Convert.ToInt32(Credenciales["MAIL_TIMEOUT"].ToString());

                int iteraciones = Destinatarios.Rows.Count % 500 == 0 ? Destinatarios.Rows.Count / 500 : ((int)Destinatarios.Rows.Count / 500) + 1;
                inicio = 0;
                fin = Destinatarios.Rows.Count > 500 ? 499 : Destinatarios.Rows.Count;

                while (iteraciones > 0)
                {
                    for (int i = inicio; i < fin; i++)
                    {
                        aux = i;

                        if (emailvalido(Destinatarios.Rows[i]["EMAIL"].ToString()))
                            Mensaje.To.Add(Destinatarios.Rows[i]["EMAIL"].ToString());
                    }

                    if (Mensaje.To.Count > 0)
                    {
                        client.Send(Mensaje);
                        Mensaje.To.Clear();
                    }

                    inicio = inicio + 500;
                    fin = fin + 500;

                    if (fin > Destinatarios.Rows.Count)
                    {
                        fin = (500 * (iteraciones - 1)) + (Destinatarios.Rows.Count - (500 * (iteraciones - 1)));
                    }

                    iteraciones--;
                }

                client.Dispose();

                return "El mensaje ha sido enviado";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EnviarEmail2(string Contenido, string Asunto, string MailBcc, DataTable Destinatarios, DataRow Credenciales, Attachment[] Adjuntos = null, string firma = "<b>UNIVERSIDAD DE LA AMAZONIA</b><br /> Calle 17 Diagonal 17 con Carrera 3F - Barrio Porvenir<br/>(+57) 8-4366160")
        {
            try
            {
                MailMessage Mensaje = new MailMessage();

                Mensaje.Subject = Asunto;

                Mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                Mensaje.Body = System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath + @"Recursos\formato.txt").Replace("{CONTENIDO}", Contenido).Replace("{FIRMA}", firma);
                Mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                Mensaje.IsBodyHtml = true;
                Mensaje.Bcc.Add(MailBcc);

                /*AGREGAMOS LOS ADJUNTOS DEL CORREO*/
                if (Adjuntos != null)
                    foreach (Attachment it in Adjuntos)
                        Mensaje.Attachments.Add(it);

                Mensaje.From = new MailAddress(Credenciales["MAIL_FROM"].ToString(), Credenciales["DISPLAYNAME"].ToString(), System.Text.Encoding.UTF8);

                SmtpClient client = new SmtpClient();

                client.Port = Convert.ToInt32(Credenciales["PORT"].ToString());
                client.Host = Credenciales["SERVER"].ToString();
                client.Credentials = new System.Net.NetworkCredential(Credenciales["MAIL"].ToString(), Credenciales["PASS"].ToString());
                if (Credenciales.Table.Columns.Contains("SSL")) client.EnableSsl = Convert.ToBoolean(Credenciales["SSL"].ToString());
                if (Credenciales.Table.Columns.Contains("MAIL_TIMEOUT")) client.Timeout = Convert.ToInt32(Credenciales["MAIL_TIMEOUT"].ToString());

                var MessageCount = 0;

                /*RECORREMOS LOS CORREOS Y COMPROBAMOS SI SON VALIDOS*/
                foreach (DataRow it in Destinatarios.Rows)
                {
                    /*VERIFICAMOS SI EL CORREO ES VALIDO*/
                    if (emailvalido(it["EMAIL"].ToString()))
                    {
                        Mensaje.To.Add(it["EMAIL"].ToString());
                        MessageCount++;
                    }

                    /*SI COMPLETAMOS EL TOPE DE CORREOS ENVIAMOS */
                    if (MessageCount > 499)
                    {
                        client.Send(Mensaje);
                        Mensaje.To.Clear();
                        MessageCount = 0;
                    }
                }
                /*EN CASO DE QUE NO SE HAYAN COMPLETADO LOS 500 CORREOS ENVIAMOS*/
                if (MessageCount > 0)
                {
                    client.Send(Mensaje);
                    Mensaje.To.Clear();
                }

                client.Dispose();

                return "El mensaje ha sido enviado";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool emailvalido(string email)
        {
            return Regex.IsMatch(email,
              @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

        }
        public string formatearContenido(string proyecto, string convocatoria, string tipo)
        {
            string contenido = "";
            contenido += "<h3>Creación de nuevo proyecto de investigación en plataforma SIGEPI</h3><br/>";

            contenido += "<p>Cordial saludo <b>VICERRECTORÍA DE INVESTIGACIONES</b>, el presente correo es para notificar la presentación de un nuevo proyecto llamado '" + proyecto + "'</p>";
            contenido += "<b>Convocatoria: </b>" + convocatoria + "<br/><b>Tipo de proyecto: <b/>" + tipo + "<br/>";
            contenido += "<br/>Muchas gracias por su atención<br/>";

            return this.ObtenerPlantillaCorreo().Replace("{0}", contenido);
            //return contenido;
        }
        public string enviarRespuestaSolicitudVehiculo(DataRow row)
        {
            string res = @"<h4>Cordial saludo {0}</h4>
                    <p>Su solicitud No. {1} de reserva de vehiculos realizada el día {2} con motivo: {3} para los dias {4} al {5}, ha sido  {6}  por {7}. </p>
                    <p><h5>Nota: </h5> {8} </p>
                    <p>Puede consultar el detalle de la solicitud en la plataforma Chairá.</p>
                    <p> Agradecemos la atención prestada.</p> ";

            string contenido = string.Format(res, row["NOMBRE"].ToString(), row["REVS_ID"].ToString(), row["REVS_FECHAREGISTRO"].ToString(), row["REVS_DESCRIPCION"].ToString(), row["REVS_FECHAINICIO"].ToString(), row["REVS_FECHAFIN"].ToString(), row["ESTADO"].ToString(), row["RVET_NOMBRE"].ToString(), row["RVSH_OBSERVACION"].ToString());
            return this.ObtenerPlantillaCorreo().Replace("{0}", contenido);

            // return res;
        }
        public string enviarCambioDeRecurso(DataRow row)
        {
            string res = @"<h4>Cordial saludo {0}</h4>
                    <p>Su solicitud No. {1} de reserva de vehiculos realizada el día {2} con motivo: {3} para los dias {4} al {5}, ha sido  {6}  por {7}. </p>
                    <p>Detalle: </p>
                    <p>Se designó el vehículo {8} y al señor {9} como conductor. </p>
                    <p>Motivo: {10} </p>
                    <p>Puede consultar el detalle de la solicitud en la plataforma Chairá.</p>
                    <p>Agradecemos la atención prestada.</p> ";

            string contenido = string.Format(res, row["NOMBRE"].ToString(), row["REVS_ID"].ToString(), row["REVS_FECHAREGISTRO"].ToString(), row["REVS_DESCRIPCION"].ToString(), row["REVS_FECHAINICIO"].ToString(),
                row["REVS_FECHAFIN"].ToString(), row["ESTADO"].ToString(), row["RVET_NOMBRE"].ToString(), row["VEHICULO"].ToString(), row["CONDUCTOR"].ToString(), row["RVSH_OBSERVACION"].ToString());
            return this.ObtenerPlantillaCorreo().Replace("{0}", contenido);

            // return res;
        }

    }
}