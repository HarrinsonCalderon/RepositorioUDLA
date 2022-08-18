using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChairaMongo;
using System.Configuration;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class iTextEvento : PdfPageEventHelper
    {

        protected PdfTemplate total;
        protected BaseFont helv;
        private int TotalPage;
        private bool MarcaAgua, EncabezadoPiePagina, bUsuario, bPaginado, EncabezadoPersonalizado = false;
        private string TextoMarcaAgua;
        private string Usuario;
        private int TipoEncabezado; // 1 - Tipo general / 2 - Formato gestion de calidad
        private string NomFor, VerFor, FecFor, CodFor;
        private float AlineaEncabezado;
        private string RutaEncabezado = "Recursos/Imagen/encabezado.png";
        private string Continua;
        private String EncabezadoMongo = String.Empty;
        private String PiePaginaMongo = String.Empty;


        MongoNoSQL MG = new MongoNoSQL("ImagenGeneral", ConfigurationManager.AppSettings["SERVER_MONGO"]);


        /// <summary>
        /// Posibilita la creacion de un encabezado y un pie de pagina con la info de la U 
        /// </summary>
        public iTextEvento()
        {
            TotalPage = 0;
            Usuario = "Chairá";
            MarcaAgua = false;
            TextoMarcaAgua = "Universidad de la Amazonia";
            EncabezadoPiePagina = true;
            TipoEncabezado = 1;
            this.Continua = null;
        }

        /// <summary>
        /// Crea encabezado de tipo general, con el numero de paginas y quien generó el archivo si se envia llos datos
        /// </summary>
        /// <param name="TP">Total de paginas</param>
        /// <param name="SiMarcadeAgua">Establece marca de agua</param>
        /// <param name="SiEncabePie">Establece encabezado y pie de pagina</param>
        /// <param name="TxtMarcaAgua">Texto para la marca de agua</param>
        /// <param name="usuario">Usuario que genera el documento</param>
        public iTextEvento(int TP = 1, bool SiMarcadeAgua = true, bool SiEncabePie = true, bool SiUsuario = true, bool SiPaginado = true, string TxtMarcaAgua = "Universidad de la Amazonia", string usuario = "Chairá", string Continua = null, String encabezadoMongo = null, String piePaginaMongos = null)
        {
            Usuario = usuario;
            TotalPage = TP;
            MarcaAgua = SiMarcadeAgua;
            EncabezadoPiePagina = SiEncabePie;
            bUsuario = SiUsuario;
            bPaginado = SiPaginado;
            TextoMarcaAgua = TxtMarcaAgua;
            TipoEncabezado = 1;
            this.Continua = Continua;
            //this.EncabezadoMongo = (String.IsNullOrEmpty(encabezadoMongo) ? "5e6a526fbfb1846b206957e0" : encabezadoMongo);
            this.EncabezadoMongo = (String.IsNullOrEmpty(encabezadoMongo) ? "621d2189df147329a00d72cd" : encabezadoMongo);
            // this.PiePaginaMongo= (String.IsNullOrEmpty(piePaginaMongos) ? "5e3b55d6bfb1844a2ca04c00" : piePaginaMongos);
            this.PiePaginaMongo = (String.IsNullOrEmpty(piePaginaMongos) ? "6165d7e3df146a095cf552e1" : piePaginaMongos);


        }

        /// <summary>
        /// Crea encabezado de tipo general, con el numero de paginas y quien generó el archivo si se envia llos datos
        /// </summary>
        /// <param name="TP">Total de paginas</param>
        /// <param name="SiMarcadeAgua">Establece marca de agua</param>
        /// <param name="SiEncabePie">Establece encabezado y pie de pagina</param>
        /// <param name="TxtMarcaAgua">Texto para la marca de agua</param>
        /// <param name="usuario">Usuario que genera el documento</param>
        public iTextEvento(float Alineaencabezado, string Rutaencabezado, int TP = 1, bool SiMarcadeAgua = true, bool SiEncabePie = true, bool SiUsuario = true, bool SiPaginado = true, string TxtMarcaAgua = "Universidad de la Amazonia", string usuario = "Chairá")
        {
            Usuario = usuario;
            TotalPage = TP;
            MarcaAgua = SiMarcadeAgua;
            EncabezadoPiePagina = SiEncabePie;
            bUsuario = SiUsuario;
            bPaginado = SiPaginado;
            TextoMarcaAgua = TxtMarcaAgua;
            AlineaEncabezado = Alineaencabezado;
            RutaEncabezado = Rutaencabezado;
            TipoEncabezado = 1;
            EncabezadoPersonalizado = true;
            this.Continua = null;
        }

        public iTextEvento(bool SUsua, bool SPag, bool SiMarcadeAgua = true, bool SiEncabePie = true, string TxtMarcaAgua = "Universidad de la Amazonia", string usuario = "Chairá")
        {
            Usuario = usuario;
            bUsuario = SUsua;
            bPaginado = SPag;
            MarcaAgua = SiMarcadeAgua;
            EncabezadoPiePagina = SiEncabePie;
            TextoMarcaAgua = TxtMarcaAgua;
            TipoEncabezado = 1;
            EncabezadoPersonalizado = false;
            this.Continua = null;
        }

        public iTextEvento(int TP, string NomFor, string CodFor, string VerFor, string FecFor)
        {
            this.TotalPage = TP;
            TipoEncabezado = (NomFor.Length == 0 ? 1 : 2);
            this.NomFor = NomFor;
            this.CodFor = CodFor;
            this.VerFor = VerFor;
            this.FecFor = FecFor;
            MarcaAgua = EncabezadoPiePagina = EncabezadoPersonalizado = false;
            this.Continua = null;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(100, 100);
            total.BoundingBox = new Rectangle(-20, -20, 100, 100);
            helv = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {

            if (TipoEncabezado == 2)
            {
                string text = "<table width='100%' align='center' border='1' style='font-size:8px;'>" +
                     "              <tr style='font-size:8px;'> " +
                     "                <td style='font-size:8px;' width='25%' rowspan='2' align='center'><img src=\"" + HttpRuntime.AppDomainAppPath + @"Recursos\Imagen\ua_pdf.png" + "\" /></td>" +
                     "                <td style='font-size:8px;' colspan='4' align='center'><b>" + NomFor + " </b></td>" +
                     "              </tr>" +
                     "              <tr style='font-size:8px;'> " +
                     "                <td style='font-size:8px;' width='25%' align='center'>CODIGO:<br/>" + CodFor + "<br/></td>" +
                     "                <td style='font-size:8px;' width='25%'  align='center'>VERSION:<br/>" + VerFor + "</td>" +
                     "                <td style='font-size:8px;' width='25%'  align='center'>FECHA:<br/>" + FecFor + "</td>" +
                     "                <td style='font-size:8px;' width='25%'  align='center'>PAGINA:<br/>" + writer.PageNumber.ToString() + " de " + TotalPage + "</td>" +
                     "              </tr>" +
                     " </table><br/>";

                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("table", "font-family", "times new roman");
                styles.LoadTagStyle("table", "font-size", "8" + "px");
                styles.LoadTagStyle("tr", "font-family", "times new roman");
                styles.LoadTagStyle("tr", "font-size", "8" + "px");
                styles.LoadTagStyle("td", "font-family", "times new roman");
                styles.LoadTagStyle("td", "font-size", "8" + "px");
                List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(text), styles);
                for (int k = 0; k < objects.Count; k++)
                    document.Add((IElement)objects[k]);
            }
        }


        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            try
            {
                cb.SaveState();
                cb.BeginText();
                string texto = "";

                const int textSize = 6;
                const int textBase = 30; // Este lo pone la informacion en la parte inferior
                const int textBases = 730; // Este lo pone la informacion en la parte superior

                #region Imagenes
                if (EncabezadoPiePagina)
                {
                    string path = HttpRuntime.AppDomainAppPath;
                    cb.SetFontAndSize(helv, 6);

                    //Consulamos la imagen en Mongo
                    MongoInfoArchivo mgfencabezado = MG.DocumentoConsultarId(EncabezadoMongo);// MG.DocumentoConsultarId("5e6a526fbfb1846b206957e0");
                    MongoInfoArchivo mgfpiepagina = MG.DocumentoConsultarId(PiePaginaMongo);// MG.DocumentoConsultarId("5e3b55d6bfb1844a2ca04c00");

                    var imgLogo = iTextSharp.text.Image.GetInstance(mgfencabezado.Archivo);
                    var pie = iTextSharp.text.Image.GetInstance(mgfpiepagina.Archivo);

                    pie.ScaleAbsolute(510, 70);
                    imgLogo.ScaleAbsolute(510, 70);

                    float posicionizquierda = EncabezadoPersonalizado ? AlineaEncabezado : document.Left;

                    imgLogo.SetAbsolutePosition(posicionizquierda, document.Top + 20);
                    cb.AddImage(imgLogo);

                    cb.AddTemplate(total, posicionizquierda + textSize, textBases);
                    pie.SetAbsolutePosition(posicionizquierda, textBase - 25);
                    cb.AddImage(pie);
                    cb.AddTemplate(total, posicionizquierda + textSize, textBase);
                }
                #endregion Pie de Pagina

                #region Continuacion de Documento
                if (Continua != null)
                {
                    if (!writer.PageNumber.ToString().Equals("1"))
                    {
                        texto = "Documento: " + Continua + " - Página " + writer.PageNumber.ToString();
                        //cb.ShowTextAligned(iTextSharp.text.Element.ALIGN_RIGHT, texto, document.LeftMargin, 90, 0);
                        cb.ShowTextAligned(iTextSharp.text.Element.ALIGN_RIGHT, texto, document.Right, document.Top, 0);
                    }
                }


                #endregion

                #region Paginas, fecha generación y usuario


                if (bUsuario)
                {
                    //Num paginas
                    ////Fecha elaboración
                    //texto = "Fecha de generación: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //cb.ShowTextAligned(iTextSharp.text.Element.ALIGN_RIGHT, texto, document.Right - 100, 90, 0);
                    //Usuario y fecha
                    texto = "Generado por: " + Usuario + ", fecha: " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt", new System.Globalization.CultureInfo("es-CO"));
                    cb.ShowTextAligned(iTextSharp.text.Element.ALIGN_LEFT, texto, document.LeftMargin, 90, 0);
                }

                if (bPaginado)
                {
                    texto = TotalPage > 1
                           ? "Página: " + writer.PageNumber.ToString() + " de " + TotalPage
                           : "Página: " + writer.PageNumber.ToString();
                    cb.ShowTextAligned(iTextSharp.text.Element.ALIGN_RIGHT, texto, document.Right, 90, 0);
                }
                #endregion Paginas, fecha generación y usuario

                #region Marca de Agua
                if (MarcaAgua)
                {
                    BaseFont bf = BaseFont.CreateFont(@"c:\windows\fonts\arial.ttf", BaseFont.CP1252, true);
                    PdfGState gs = new PdfGState { FillOpacity = 0.35F, StrokeOpacity = 0.35F };
                    MarcaDeAgua(bf, document.PageSize, cb, gs);
                }
                #endregion Marca de Agua


                //cb.RestoreState();
                //cb.EndText();

            }
            catch (Exception ex) { }
            finally
            {
                cb.EndText();
                cb.RestoreState();
            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            //totcountPage = writer.PageNumber;
            //MessageBox.Show(p.ToString());     
            //int i=document.PageCount
        }

        private void MarcaDeAgua(BaseFont bf, Rectangle tamPagina, PdfContentByte over, PdfGState gs)
        {

            over.SetGState(gs);

            over.SetRGBColorFill(220, 220, 220);

            over.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_STROKE);

            over.SetFontAndSize(bf, 46);

            Single anchoDiag =

                (Single)Math.Sqrt(Math.Pow((tamPagina.Height - 120), 2)

                + Math.Pow((tamPagina.Width - 60), 2));

            Single porc = (Single)100

                * (anchoDiag / bf.GetWidthPoint(TextoMarcaAgua, 46));

            over.SetHorizontalScaling(porc);

            // /
            double angPage = (1) * Math.Atan((tamPagina.Height - 60) / (tamPagina.Width - 60));
            over.SetTextMatrix((float)Math.Cos(angPage), (float)Math.Sin(angPage), (float)((-1F) * Math.Sin(angPage)), (float)Math.Cos(angPage), 30F, (float)30F);
            // \
            //double angPage = (-1) * Math.Atan((tamPagina.Height - 60) / (tamPagina.Width - 60));
            //over.SetTextMatrix((float)Math.Cos(angPage), (float)Math.Sin(angPage), (float)((-1F) * Math.Sin(angPage)), (float)Math.Cos(angPage), 30F,(float)tamPagina.Height - 60);

            over.ShowText(TextoMarcaAgua);
        }
    }
}