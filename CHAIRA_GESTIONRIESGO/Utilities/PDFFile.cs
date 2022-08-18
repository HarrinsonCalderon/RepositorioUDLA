using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class PDFFile
    {

        /// <summary>
        /// Crea archivo pdf, usando la libreria itextSharp
        /// </summary>
        /// <param name="html">Contenido del documento</param>
        /// <param name="tamano">Tamaño de la pagina a emplear en el documento CARTA, OFICIO</param>
        /// <param name="MIzq">Margen izquierda</param>
        /// <param name="MDer">Margen derecha</param>
        /// <param name="MArr">Margen superior</param>
        /// <param name="MAbaj">Margen inferior</param>
        /// <param name="TamFuente">Tamaño de la fuente</param>
        /// <param name="EncabezadoPiePagina">Define la impresión del encabezado y pie de pagina</param>
        /// <param name="MarcaAgua">Define impresión de la marca de agua</param>
        /// <param name="TxtMArcaAgua">Texto de la marca de agua</param>
        /// <param name="Usuario">Usuario que genera el archivo</param>
        /// <returns></returns>
        public byte[] CrearDocumento(string html, string Orientacion = "VERTICAL", string tamano = "CARTA", float MIzq = 30, float MDer = 20, float MArr = 25, float MAbaj = 30, int TamFuente = 8, bool EncabezadoPiePagina = false, bool MarcaAgua = false, string TxtMArcaAgua = "Universidad de la Amazonia", string Usuario = "Chairá", bool Paginado = true, bool MostrarUsuario = true, string Continua = null, String EncabezadoMongo = null, String PiePaginaMongo = null)
        {
            try
            {
                var TamanoPagina = this.TamanoPagina(tamano);
                Document document = new Document(TamanoPagina, MIzq, MDer, MArr, MAbaj);

                if (Orientacion.Equals("VERTICAL"))
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                else if (Orientacion.Equals("HORIZONTAL"))
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                else if (Orientacion.Equals("VERTICALO"))
                    document.SetPageSize(iTextSharp.text.PageSize.LEGAL);


                string FileName = "DocPaginas_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

                int cantidadpaginas = CantidadDePaginas(html, TamanoPagina, MIzq, MDer, MArr, MAbaj, TamFuente);

                document.NewPage();

                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("body", "font-family", "times new roman");
                styles.LoadTagStyle("body", "font-size", TamFuente + "px");

                MemoryStream ms = new MemoryStream();
                PdfWriter pdfW = PdfWriter.GetInstance(document, ms);

                if (EncabezadoPiePagina)
                {

                    pdfW.PageEvent = new iTextEvento(cantidadpaginas, MarcaAgua, EncabezadoPiePagina, MostrarUsuario, Paginado, TxtMArcaAgua, Usuario, Continua, EncabezadoMongo, PiePaginaMongo);
                }
                document.NewPage();
                List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), styles);
                document.Open();
                for (int k = 0; k < objects.Count; k++)
                    document.Add((IElement)objects[k]);


                #region nuevo
                #region Limpia caracteres sensibles a error
                //html = html
                //            .Replace("</br>", "<br/>")
                //            .Replace("<br>", "<br/>").Trim();
                #endregion
                #endregion
                #region Reemplazar atributos html compatible
                //XMLWorkerHelper xml = XMLWorkerHelper.GetInstance();
                //using (var msCSS = new MemoryStream(Encoding.UTF8.GetBytes(path + "Resources\\Estilos\\Fuente.css")))
                // {
                //xml.ParseXHtml(pdfW, document, stringToStream(html), System.Text.Encoding.UTF8);
                // }

                #endregion

                document.Close();

                byte[] data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Stream stringToStream(string txt)
        {
            var stream = new MemoryStream();
            var w = new StreamWriter(stream);
            w.Write(txt);
            w.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Crea archivo pdf, usando la libreria itextSharp
        /// </summary>
        /// <param name="html">Contenido del documento</param>
        /// <param name="tamano">Tamaño de la pagina a emplear en el documento CARTA, OFICIO</param>
        /// <param name="MIzq">Margen izquierda</param>
        /// <param name="MDer">Margen derecha</param>
        /// <param name="MArr">Margen superior</param>
        /// <param name="MAbaj">Margen inferior</param>
        /// <param name="TamFuente">Tamaño de la fuente</param>
        /// <param name="EncabezadoPiePagina">Define la impresión del encabezado y pie de pagina</param>
        /// <param name="MarcaAgua">Define impresión de la marca de agua</param>
        /// <param name="TxtMArcaAgua">Texto de la marca de agua</param>
        /// <param name="Usuario">Usuario que genera el archivo</param>
        /// <returns></returns>
        public byte[] CrearDocumento(string html, float AlineaEncabezado, string RutaEncabezado, string Orientacion = "VERTICAL", string tamano = "CARTA", float MIzq = 30, float MDer = 20, float MArr = 15, float MAbaj = 30, int TamFuente = 8, bool EncabezadoPiePagina = false, bool MarcaAgua = false, string TxtMArcaAgua = "Universidad de la Amazonia", string Usuario = "Chairá", bool Paginado = true, bool MostrarUsuario = true)
        {
            try
            {
                var TamanoPagina = this.TamanoPagina(tamano);
                Document document = new Document(TamanoPagina, MIzq, MDer, MArr, MAbaj);

                if (Orientacion.Equals("VERTICAL"))
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                else if (Orientacion.Equals("HORIZONTAL"))
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                string FileName = "DocPaginas_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

                int cantidadpaginas = CantidadDePaginas(html, TamanoPagina, MIzq, MDer, MArr, MAbaj, TamFuente);

                document.NewPage();

                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("body", "font-family", "times new roman");
                styles.LoadTagStyle("body", "font-size", TamFuente + "px");

                MemoryStream ms = new MemoryStream();
                PdfWriter pdfW = PdfWriter.GetInstance(document, ms);

                if (EncabezadoPiePagina)
                    pdfW.PageEvent = new iTextEvento(AlineaEncabezado, RutaEncabezado, cantidadpaginas, MarcaAgua, EncabezadoPiePagina, MostrarUsuario, Paginado, TxtMArcaAgua, Usuario);

                document.NewPage();
                List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), styles);
                document.Open();
                for (int k = 0; k < objects.Count; k++)
                    document.Add((IElement)objects[k]);
                document.Close();

                byte[] data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Crea Pdf con encabezado del formato de gestion de calidad
        /// </summary>
        /// <param name="html">Contenido en html del documento</param>
        /// <param name="NomFor">Nombre del formato</param>
        /// <param name="CodFor">Codigo del formato</param>
        /// <param name="VerFor">Version del formato</param>
        /// <param name="FeFor"><Fecha del formato/param>
        /// <param name="MIzq">Margen izquierda</param>
        /// <param name="MDer">Margen derecha</param>
        /// <param name="MArr">Margen superior</param>
        /// <param name="MAbaj">Margen inferior</param>
        /// <param name="TamFuente">Tamaño de fuente</param>
        /// <returns></returns>
        public byte[] CrearFormato(string html, string NomFor, string CodFor, string VerFor, string FeFor, string tamano = "CARTA", float MIzq = 30, float MDer = 20, float MArr = 10, float MAbaj = 30, int TamFuente = 8)
        {
            try
            {
                var TamanoPagina = this.TamanoPagina(tamano);
                Document document = new Document(TamanoPagina, MIzq, MDer, MArr, MAbaj);

                int cantidadpaginas = CantidadDePaginas(html, TamanoPagina, MIzq, MDer, MArr, MAbaj, TamFuente);

                document.NewPage();

                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("body", "font-family", "times new roman");
                styles.LoadTagStyle("body", "font-size", TamFuente + "px");
                styles.LoadStyle("redBigText", "color", "#ff0000");


                MemoryStream ms = new MemoryStream();
                PdfWriter pdfW = PdfWriter.GetInstance(document, ms);

                pdfW.PageEvent = new iTextEvento(cantidadpaginas, NomFor, CodFor, VerFor, FeFor);

                document.NewPage();
                var objects = HTMLWorker.ParseToList(new StringReader(html), styles);
                document.Open();

                foreach (var obj in objects)
                {
                    if (obj is PdfPTable)
                    {
                        (obj as PdfPTable).SplitLate = false;
                    }
                    document.Add(obj as IElement);
                }
                //for (int k = 0; k < objects.Count; k++)
                //{
                //    //Console.WriteLine($"{(IElement)objects[k].GetType()}");              
                //    document.Add((IElement)objects[k]);
                //}

                document.Close();

                byte[] data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int CantidadDePaginas(string html, Rectangle TamanoPagina, float MIzq, float MDer, float MArr, float MAbaj, int TamFuente, int tipoDoc = 1, string NomFor = "")
        {
            try
            {
                Document document = new Document(TamanoPagina, MIzq, MDer, MArr, MAbaj);

                string FileName = "DocPaginas_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";

                document.NewPage();

                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("body", "font-family", "times new roman");
                styles.LoadTagStyle("body", "font-size", TamFuente + "px");

                PdfWriter pdfW = PdfWriter.GetInstance(document, new FileStream(HttpRuntime.AppDomainAppPath + @"Recursos\Pdf\" + FileName, FileMode.OpenOrCreate));

                pdfW.PageEvent = new iTextEvento(0, NomFor, "", "", "");

                document.NewPage();
                List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), styles);
                document.Open();
                for (int k = 0; k < objects.Count; k++)
                    document.Add((IElement)objects[k]);
                document.Close();

                PdfReader doc = new PdfReader(HttpRuntime.AppDomainAppPath + @"Recursos\Pdf\" + FileName);
                int r = doc.NumberOfPages;
                doc.Close();
                File.Delete(HttpRuntime.AppDomainAppPath + @"Recursos\Pdf\" + FileName);
                return r;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        private Rectangle TamanoPagina(string tamano)
        {
            switch (tamano)
            {
                case "CARTA":
                    return PageSize.LETTER;
                case "OFICIO":
                    return new Rectangle(0f, 0f, 612f, 936);
                case "OFICIOR":
                    return PageSize.LEGAL;
                default:
                    return PageSize.LETTER;
            }
        }

        public byte[] ExportarExcel(string nombre_columnas, DataTable datos)
        {
            try
            {
                MemoryStream fs = new MemoryStream();
                StreamWriter w = new StreamWriter(fs);
                string comillas = char.ConvertFromUtf32(34);
                StringBuilder html = new StringBuilder();
                html.Append(@"<!DOCTYPE html PUBLIC" + comillas + "-//W3C//DTD XHTML 1.0 Transitional//EN" + comillas + " " + comillas + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" + comillas + ">");
                html.Append(@"<html xmlns=" + comillas + "http://www.w3.org/1999/xhtml" + comillas + ">");
                html.Append(@"<head>");
                html.Append(@"<meta http-equiv=" + comillas + "Content-Type" + comillas + "content=" + comillas + "text/html; charset=utf-8" + comillas + " />");
                html.Append(@"<title>Untitled Document</title>");
                html.Append(@"</head>");
                html.Append(@"<body>");

                //Generando encabezados del archivo
                html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 border=1 BORDERCOLOR=" + comillas + "#333366" + comillas + " bgcolor=" + comillas + "#FFFFFF" + comillas + ">");
                html.Append(@"<tr> <b>");

                string[] columnas = nombre_columnas.Split('/');

                for (int i = 0; i < columnas.Length; i++)
                    html.Append(@"<th bgcolor=" + comillas + "#95BE76" + comillas + ">" + columnas[i] + "</th>");

                html.Append(@"</b> </tr>");

                //Generando datos del archivo
                for (int i = 0; i < datos.Rows.Count; i++)
                {
                    string color = i % 2 == 0 ? "#FAFAFA" : "#EDEDED";
                    html.Append(@"<tr>");
                    for (int j = 0; j < columnas.Length; j++)
                        html.Append(@"<td bgcolor=" + comillas + color + comillas + ">" + datos.Rows[i][columnas[j]].ToString() + "</td>");

                    html.Append(@"</tr>");
                }

                html.Append(@"</body>");
                html.Append(@"</html>");

                w.Write(html.ToString());
                w.Close();
                return fs.GetBuffer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}