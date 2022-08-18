using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using ClosedXML.Excel;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class ExcelClosedXML
    {
        //XLWorkbook libro;

        public MemoryStream DataTableAExcel(DataTable T)
        {
            var ArchivoExcel = new XLWorkbook();
            var Libro1 = ArchivoExcel.Worksheets.Add("Libro1");

            #region AGREGA NOMBRE DE COLUMNAS
            for (int i = 0; i < T.Columns.Count; i++)
            {
                Libro1.Cell(1, (i + 1)).SetValue(T.Columns[i].ColumnName.Replace("_", " "));
                //Libro1.Style.Border.set
            }
            Libro1.Range(1, 1, 1, T.Columns.Count).Style
             .Font.SetFontSize(11)
             .Font.SetBold(true)
             //.Font.SetFontColor(XLColor.White)
             .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
             .Fill.SetBackgroundColor(XLColor.BabyBlue);
            Libro1.RangeUsed().SetAutoFilter();
            // worksheet.Range("B2:E5")
            //.SetValue("Hi!")
            //.Style.Border.SetOutsideBorder(XLBorderStyleValues.Dotted);


            #endregion
            for (int Columna = 0; Columna < T.Columns.Count; Columna++)
            {
                for (int Fila = 0; Fila < T.Rows.Count; Fila++)
                {
                    Libro1.Cell((Fila + 2), (Columna + 1)).SetValue(T.Rows[Fila][Columna]);
                }
            }
            //Libro1.Cell(134, 1).Value = "frank";
            Libro1.Range(7, 21, 8, 21).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //Libro1.Cell("A1").Value = "Frank";
            MemoryStream memoryStream = new MemoryStream();
            ArchivoExcel.SaveAs(memoryStream);
            //using (var memoryStream = new MemoryStream())
            //{

            //    memoryStream.WriteTo(response.OutputStream);
            //}
            //ArchivoExcel.SaveAs("D:\\wwwroot\\Reporte.xlsx");
            return memoryStream;
        }
        public MemoryStream DataTableAExcel(ParametrosDataTableExcel Param)
        {

            var ArchivoExcel = new XLWorkbook();
            //foreach (String Libros in Param.Libros)
            //{
            var Libro = ArchivoExcel.Worksheets.Add("Hoja1");
            switch (Param.Data.GetType().Name)
            {
                case "DataTable":
                    #region DATATBLE
                    #region AGREGA NOMBRE DE COLUMNAS
                    DataTable T = (DataTable)Param.Data;
                    for (int i = 0; i < T.Columns.Count; i++)
                    {
                        Libro.Cell(1, (i + 1)).SetValue(T.Columns[i].ColumnName.Replace("_", " "));
                        //Libro1.Style.Border.set
                    }
                    Libro.Range(1, 1, 1, T.Columns.Count).Style
                     .Font.SetFontSize(11)
                     .Font.SetBold(true)
                     //.Font.SetFontColor(XLColor.White)
                     .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                     .Fill.SetBackgroundColor(XLColor.BabyBlue);
                    Libro.RangeUsed().SetAutoFilter();
                    // worksheet.Range("B2:E5")
                    //.SetValue("Hi!")
                    //.Style.Border.SetOutsideBorder(XLBorderStyleValues.Dotted);


                    #endregion
                    #region DATA
                    for (int Columna = 0; Columna < T.Columns.Count; Columna++)
                    {
                        for (int Fila = 0; Fila < T.Rows.Count; Fila++)
                        {
                            try
                            {
                                Libro.Cell((Fila + 2), (Columna + 1)).SetValue(T.Rows[Fila][Columna]);
                            }
                            catch (Exception) { }
                        }
                    }
                    #endregion
                    #region COMBINAR

                    if (Param.CombinarCelda != null)
                    {
                        foreach (CombinaCelda CC in Param.CombinarCelda)
                        {
                            Libro.Range(CC.Fil1, CC.Col1, CC.Fil2, CC.Col2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        }
                    }
                    #endregion
                    //Libro1.Cell(134, 1).Value = "frank";
                    #endregion
                    break;
            }
            //}
            //Libro1.Cell("A1").Value = "Frank";
            MemoryStream memoryStream = new MemoryStream();
            ArchivoExcel.SaveAs(memoryStream);
            //using (var memoryStream = new MemoryStream())
            //{

            //    memoryStream.WriteTo(response.OutputStream);
            //}
            //ArchivoExcel.SaveAs("D:\\wwwroot\\Reporte.xlsx");

            return memoryStream;
        }
        public MemoryStream CrearExcel(ParametrosExcel Param)
        {
            XLWorkbook ArchivoExcel = new XLWorkbook();
            foreach (String Libro in Param.Data.Libros)//Recorre los libros
            {
                IXLWorksheet Libro1 = ArchivoExcel.Worksheets.Add(Libro);

                foreach (DataX itemEX in Param.Data.Lista)//.Lista.Libros.Where(P => P.Libro  == Libro))//Escribir informacion de cada libro
                {
                    switch (itemEX.Data.GetType().Name)
                    {
                        case "EXEscribir":
                            EscribirAExcel(ref Libro1, (EXEscribir)itemEX.Data);
                            break;
                        case "EXTabla":
                            DataTableAExcel(ref Libro1, (EXTabla)itemEX.Data);
                            break;
                    }
                }

                #region DEFINE EL ANCHO DE COLUMNAS EN LA HOJA
                if (Param.AnchoCol != null)
                    foreach (AXAnchoColumna itemAC in Param.AnchoCol.Where(P => P.Libro == Libro))
                    {
                        Libro1.Column(itemAC.Col).Width = itemAC.Ancho;
                    }
                else
                    Libro1.Columns().AdjustToContents();

                #endregion
            }
            //libro = ArchivoExcel;
            MemoryStream memoryStream = new MemoryStream();
            ArchivoExcel.SaveAs(memoryStream);
            return memoryStream;
        }

        //public void FormatoCeldas(ref XLWorkbook LIBRO,  int FilI, int FilF, int ColI, int ColF, string hoja, string formato) 
        //{
        //    for (int i = FilI; i < FilF; i++)
        //    {
        //        for (int j = ColI; j < ColF; j++)
        //        {
        //            LIBRO.Worksheet(hoja).Cell(i, j).Style.NumberFormat.Format = formato;
        //            LIBRO.Worksheet(hoja).Cell(i, j).DataType = XLCellValues.Number;
        //        }
        //    }
        //}


        public void EscribirAExcel(ref IXLWorksheet Libro, EXEscribir Param)
        {
            Libro.Cell(Param.FilI, Param.ColI).SetValue(Param.Data);
            #region ALINEAR TEXTO HORIZONTAL
            Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = HAlinearCelda(Param.HAlinear);
            //switch (Param.HAlinear)
            //{
            //    case Halinear.Izquierda:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            //        break;
            //    case Halinear.Centro:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        break;
            //    case Halinear.Derecha:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //        break;
            //    case Halinear.Justificado:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Justify;
            //        break;
            //    case Halinear.Automatico:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Fill;
            //        break;
            //}

            #endregion
            #region ALINEAR TEXTO VERTICAL
            Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = VAlinearCelda(Param.VAlinear);
            //switch (Param.VAlinear)
            //{
            //    case Valinear.Abajo:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = XLAlignmentVerticalValues.Bottom;
            //        break;
            //    case Valinear.Centro:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //        break;
            //    case Valinear.Arriba:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            //        break;
            //    case Valinear.Justificado:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = XLAlignmentVerticalValues.Justify;
            //        break;
            //    case Valinear.Distribuido:
            //        Libro.Cell(Param.FilI, Param.ColI).Style.Alignment.Vertical = XLAlignmentVerticalValues.Distributed;
            //        break;
            //}
            #endregion
            #region TAMAÑO LETRA
            Libro.Cell(Param.FilI, Param.ColI).Style.Font.FontSize = Param.TLetra;
            #endregion
            #region COMBINAR CELDA
            if (Param.FilF > 0 && Param.ColF > 0)
                Libro.Range(Param.FilI, Param.ColI, Param.FilF, Param.ColF).Merge();
            #endregion

            #region FONDO
            if (!string.IsNullOrEmpty(Param.ColorFondo))
                Libro.Cell(Param.FilI, Param.ColI).Style.Fill.SetBackgroundColor(XLColor.FromHtml(Param.ColorFondo));
            #endregion

            #region NEGRILLA
            Libro.Cell(Param.FilI, Param.ColI).Style.Font.SetBold(Param.Negrilla);
            #endregion

            #region CURSIVA
            Libro.Cell(Param.FilI, Param.ColI).Style.Font.SetItalic(Param.Cursiva);
            #endregion

            #region FORMATO
            if (Param.Formato != null)
            {
                switch (Param.Formato.TipoFormato)
                {
                    case TipoFormatoCelda.Numerico:
                        Libro.Cell(Param.FilI, Param.ColI).Style.NumberFormat.Format = Param.Formato.Formato;
                        break;
                }
            }
            #endregion

            #region BORDER COLOR
            if (Param.Border)
            {
                var rangeLine = Libro.Range(Param.FilI, Param.ColI, Param.FilF, Param.ColF);

                rangeLine.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                rangeLine.Style.Border.BottomBorderColor = Param.BorderColor;
            }

            #endregion


        }
        public void DataTableAExcel(ref IXLWorksheet Libro, EXTabla Param)
        {

            double[] Sum = new double[Param.ColSuma.Length];
            double[] resumen = new double[] { };

            if (Param.Resumen != null)
            {
                resumen = new double[Param.Resumen.Count];

                for (int i = 0; i < Param.Resumen.Count; i++)
                {
                    switch (Param.Resumen[i].Opcion)
                    {
                        case OpcionResumen.Minimo:
                            resumen[i] = 999999999999;
                            break;
                        default:
                            resumen[i] = 0;
                            break;
                    }
                }
            }

            switch (Param.Data.GetType().Name)
            {
                case "DataTable":
                    #region DATABLE
                    DataTable T = (DataTable)Param.Data;
                    #region DEFINE TITULO DE LA TABLA
                    if (!String.IsNullOrEmpty(Param.Titulo))
                    {
                        Libro.Cell(Param.Fil, Param.Col).SetValue(Param.Titulo)
                        .Style.Font.SetFontSize(Param.Tletra)
                        .Font.SetBold(true)
                        .Fill.SetBackgroundColor(XLColor.FromHtml("#F1FFF1"))
                        .Alignment.SetHorizontal(HAlinearCelda(Param.TituloHAlinear))
                        .Alignment.SetVertical(VAlinearCelda(Param.TituloVAlinear))
                        .Border.SetBottomBorderColor(XLColor.FromHtml("#DDDDDE"))
                        .Border.SetBottomBorder(XLBorderStyleValues.Thin);
                        Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Merge();


                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Font.Bold = true;
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F1FFF1"));
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Alignment.SetHorizontal(HAlinearCelda(Param.TituloHAlinear));
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Alignment.SetVertical(VAlinearCelda(Param.TituloVAlinear));
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDE");
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        //Libro.Range(Param.Fil, Param.Col, Param.Fil, ((T.Columns.Count + Param.Col) - 1)).Merge();
                        Param.Fil = Param.Fil + 1;
                    }
                    #endregion
                    #region AGREGA NOMBRE DE COLUMNAS

                    for (int i = Param.Col; i < (T.Columns.Count + Param.Col); i++)
                    {
                        Libro.Cell(Param.Fil, i).SetValue(T.Columns[(i - Param.Col)].ColumnName.Replace("_", " "));
                        Libro.Cell(Param.Fil, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Libro.Cell(Param.Fil, i).Style.Font.Bold = true;
                        Libro.Cell(Param.Fil, i).Style.Fill.SetBackgroundColor(XLColor.FromHtml(Param.FondoEncabezado));
                        //Libro1.Style.Border.set
                    }
                    //Libro.Range(1, 1, 1, T.Columns.Count).Style
                    // .Font.SetFontSize(11)
                    // .Font.SetBold(true)
                    //.Font.SetFontColor(XLColor.White)
                    // .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    // .Fill.SetBackgroundColor(XLColor.BabyBlue);
                    //Libro.RangeUsed().SetAutoFilter();
                    #endregion
                    #region DATA
                    for (int Fila = Param.Fil; Fila < (T.Rows.Count + Param.Fil); Fila++)
                    {
                        #region DEFINE STRIPEROWS TABLA
                        if (Param.StripeRows)
                        {
                            if ((Fila % 2) == 0)
                                Libro.Range(Fila + 1, Param.Col, Fila + 1, ((T.Columns.Count + Param.Col) - 1)).Style.Fill.SetBackgroundColor(XLColor.FromHtml(Param.FondoStripe));
                        }
                        #endregion
                        for (int Columna = Param.Col; Columna < (T.Columns.Count + Param.Col); Columna++)
                        {
                            try
                            {
                                Libro.Cell((Fila + 1), Columna).SetValue(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)]);
                                Libro.Cell((Fila + 1), Columna).Style.Font.SetFontSize(Param.Tletra);

                                #region SUMA LAS COLUMNAS EN CASO DE QUE SE REQUIERA
                                if (Param.Resumen != null)
                                {
                                    if (Param.Resumen.Count > 0)
                                    {
                                        int pos = -1;

                                        for (int i = 0; i < Param.Resumen.Count; i++)
                                        {
                                            if (Param.Resumen[i].Pos == (Columna - Param.Col))
                                            {
                                                pos = i;
                                                break;
                                            }
                                        }

                                        if (pos >= 0)
                                        {
                                            if (!String.IsNullOrEmpty(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()))
                                            {
                                                switch (Param.Resumen[pos].Opcion)
                                                {
                                                    case OpcionResumen.Suma:
                                                        resumen[pos] += Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString());
                                                        break;
                                                    case OpcionResumen.Contador:
                                                        resumen[pos]++;
                                                        break;
                                                    case OpcionResumen.Maximo:
                                                        resumen[pos] = (Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()) > resumen[pos]) ? Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()) : resumen[pos];
                                                        break;
                                                    case OpcionResumen.Minimo:
                                                        resumen[pos] = (Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()) < resumen[pos]) ? Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()) : resumen[pos];
                                                        break;
                                                }

                                            }
                                        }
                                    }
                                }

                                if (Param.ColSuma.Length > 0)
                                {
                                    int Pos = Array.IndexOf(Param.ColSuma, (Columna - Param.Col));
                                    if (Pos >= 0)
                                    {
                                        try
                                        {
                                            if (!String.IsNullOrEmpty(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString()))
                                                Sum[Pos] += Double.Parse(T.Rows[(Fila - Param.Fil)][(Columna - Param.Col)].ToString());
                                        }
                                        catch (Exception) { }
                                    }
                                }

                                #endregion

                                #region FORMATEA LAS COLUMNAS

                                if (Param.FormatoCelda != null)
                                {
                                    List<EXTablaFormato> L = Param.FormatoCelda.Where(P => P.Columna == Columna).ToList();
                                    if (L.Count > 0)
                                    {
                                        switch (L[0].TipoFormato)
                                        {
                                            case TipoFormatoCelda.Numerico:
                                                Libro.Cell(Fila + 1, Columna).Style.NumberFormat.Format = L[0].Formato;
                                                break;
                                        }
                                    }
                                }

                                #endregion

                            }
                            catch (Exception) { }
                        }
                    }
                    #endregion
                    #region LINEA CELDA
                    Libro.Range(Param.Fil, Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    Libro.Range(Param.Fil, Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDE");
                    Libro.Range(Param.Fil, Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    Libro.Range(Param.Fil, Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDE");
                    #endregion
                    #region ESCRIBIR LA SUMATORIA DE LAS COLUMNAS

                    if (Param.Resumen != null)
                    {
                        if (Param.Resumen.Count > 0)
                        {
                            for (int i = 0; i < resumen.Length; i++)
                            {
                                Libro.Cell(((T.Rows.Count + Param.Fil) + 1), (Param.Resumen[i].Pos + Param.Col)).SetValue(((Param.Resumen[i].Opcion != OpcionResumen.Nada) ? "" + resumen[i] : "") + ((Param.Resumen[i].Formato.TipoFormato == TipoFormatoCelda.SinFormato) ? Param.Resumen[i].Texto : ""));

                                switch (Param.Resumen[i].Formato.TipoFormato)
                                {
                                    case TipoFormatoCelda.Numerico:
                                        Libro.Cell(((T.Rows.Count + Param.Fil) + 1), (Param.Resumen[i].Pos + Param.Col)).Style.NumberFormat.Format = Param.Resumen[i].Formato.Formato;
                                        break;
                                }

                            }
                        }
                    }

                    if (Param.ColSuma.Length > 0)
                    {
                        for (int i = 0; i < Sum.Length; i++)
                        {
                            Libro.Cell(((T.Rows.Count + Param.Fil) + 1), (Param.ColSuma[i] + Param.Col)).SetValue(Sum[i]);
                        }

                        //Libro.Range(((T.Rows.Count + Param.Fil) + 1), Param.Col, ((T.Rows.Count + Param.Fil) + 1), (T.Columns.Count + Param.Col)).Merge();//.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center);                        

                        // Libro.Cell(3, 1).SetFormulaA1("=SUMA(J5:J23)");
                    }
                    if (Param.Resumen != null)
                    {
                        if (Param.Resumen.Count > 0)
                        {
                            Libro.Range(((T.Rows.Count + Param.Fil) + 1), Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Font.SetBold(true)
                            .Fill.SetBackgroundColor(XLColor.FromHtml(Param.FondoTotal))
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                            .Font.SetFontSize(Param.Tletra);
                        }
                    }

                    if (Param.ColSuma.Length > 0)
                    {
                        Libro.Range(((T.Rows.Count + Param.Fil) + 1), Param.Col, ((T.Rows.Count + Param.Fil) + 1), ((T.Columns.Count + Param.Col) - 1)).Style.Font.SetBold(true)
                        .Fill.SetBackgroundColor(XLColor.FromHtml(Param.FondoTotal))
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        .Font.SetFontSize(Param.Tletra);
                    }

                    #endregion
                    #region COMBINAR

                    //if (Param.CombinarCelda != null)
                    //{
                    //    foreach (CombinaCelda CC in Param.CombinarCelda)
                    //    {
                    //        Libro.Range(CC.Fil1, CC.Col1, CC.Fil2, CC.Col2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //    }
                    //}
                    #endregion
                    //Libro1.Cell(134, 1).Value = "frank";
                    #endregion
                    break;
            }

            //Libro1.Cell("A1").Value = "Frank";
            //using (var memoryStream = new MemoryStream())
            //{

            //    memoryStream.WriteTo(response.OutputStream);
            //}
            //ArchivoExcel.SaveAs("D:\\wwwroot\\Reporte.xlsx");


        }



        private XLAlignmentHorizontalValues HAlinearCelda(Halinear Op)
        {
            #region ALINEAR TEXTO HORIZONTAL
            switch (Op)
            {
                case Halinear.Izquierda: return XLAlignmentHorizontalValues.Left;
                case Halinear.Centro: return XLAlignmentHorizontalValues.Center;
                case Halinear.Derecha: return XLAlignmentHorizontalValues.Right;
                case Halinear.Justificado: return XLAlignmentHorizontalValues.Justify;
                case Halinear.Automatico: return XLAlignmentHorizontalValues.Fill;
                default: return XLAlignmentHorizontalValues.Left;
            }
            #endregion
        }
        private XLAlignmentVerticalValues VAlinearCelda(Valinear Op)
        {
            switch (Op)
            {
                case Valinear.Abajo: return XLAlignmentVerticalValues.Bottom;
                case Valinear.Centro: return XLAlignmentVerticalValues.Center;
                case Valinear.Arriba: return XLAlignmentVerticalValues.Top;
                case Valinear.Justificado: return XLAlignmentVerticalValues.Justify;
                case Valinear.Distribuido: return XLAlignmentVerticalValues.Distributed;
                default: return XLAlignmentVerticalValues.Center;
            }
        }
    }



    public class EXLista
    {
        public EXLista()
        {
            this._Lista = new List<DataX>();
        }
        private List<DataX> _Lista { get; set; }
        public String[] Libros { get { return _Lista.Select(P => P.Libro).Distinct().ToArray(); } }
        public List<DataX> Lista { get { return _Lista; } }
        public void Add(object Data, String Libro)
        {
            this._Lista.Add(new DataX { Data = Data, Libro = Libro });
        }
    }
    public class DataX
    {
        public object Data { get; set; }
        public String Libro { get; set; }
    }
    public class ParametroListaDataExcel
    {
        public int FilInicial { get; set; }
        public int ColInicial { get; set; }
        public Object Data { get; set; }
        public List<CombinaCelda> CombinarCelda { get; set; }
    }
    public class ParametrosExcel
    {
        //public Object Data { get; set; }
        //public List<String> Libros { get; set; }
        //public List<CombinaCelda> CombinarCelda { get; set; }
        public EXLista Data { get; set; }
        public List<AXAnchoColumna> AnchoCol { get; set; }
    }
    public class ParametrosDataTableExcel
    {
        public Object Data { get; set; }
        public List<String> Libros { get; set; }
        public List<CombinaCelda> CombinarCelda { get; set; }
        //public EXLista Escribir { get; set; }

    }
    public class CombinaCelda
    {
        public CombinaCelda(int Fila1, int Columna1, int Fila2, int Columna2)
        {
            this.Fil1 = Fila1;
            this.Col1 = Columna2;
            this.Fil2 = Fila2;
            this.Col2 = Columna2;
        }
        public int Fil1 { get; set; }
        public int Col1 { get; set; }
        public int Fil2 { get; set; }
        public int Col2 { get; set; }
    }
    public class AXAnchoColumna
    {
        public AXAnchoColumna(int columna, float ancho, String libro)
        {
            this.Col = columna;
            this.Ancho = ancho;
            this.Libro = libro;
        }
        public int Col { get; set; }
        public float Ancho { get; set; }
        public String Libro { get; set; }
    }
    public class EXEscribir
    {
        public EXEscribir()
        {
            this.Data = String.Empty;
            this.HAlinear = Halinear.Izquierda;
            this.VAlinear = Valinear.Abajo;
            this.TLetra = 10;
            this.FilI = 1;
            this.ColI = 1;
            this.FilF = -1;
            this.ColF = -1;
            this.ColorFondo = string.Empty;
            this.Negrilla = false;
            this.Border = false;
            this.Cursiva = false;
        }

        public String Data { get; set; }
        public int FilI { get; set; }
        public int ColI { get; set; }
        public int FilF { get; set; }
        public int ColF { get; set; }
        public Halinear HAlinear { get; set; }
        public Valinear VAlinear { get; set; }
        public int TLetra { get; set; }
        public string ColorFondo { get; set; }
        public bool Negrilla { get; set; }
        public bool Cursiva { get; set; }
        public EXTablaFormato Formato { get; set; }
        public bool Border { get; set; }
        public XLColor BorderColor { get; set; }

    }
    public class EXTabla
    {
        public EXTabla()
        {
            this.Data = null;
            this.Titulo = String.Empty;
            this.StripeRows = false;
            this.TituloHAlinear = Halinear.Izquierda;
            this.TituloVAlinear = Valinear.Centro;
            this.Tletra = 10;
            this.FondoEncabezado = "#A5D6A7";
            this.FondoStripe = "#C8E6C9";
            this.FondoTotal = "#E6E6E6";
        }
        public Object Data { get; set; }
        public int Fil { get; set; }
        public int Col { get; set; }
        public int[] ColSuma { get; set; }
        public List<EXTablaResumen> Resumen { get; set; }
        public string Titulo { get; set; }
        public Halinear TituloHAlinear { get; set; }
        public Valinear TituloVAlinear { get; set; }
        public Boolean StripeRows { get; set; }
        public int Tletra { get; set; }
        public List<EXTablaFormato> FormatoCelda { get; set; }
        public string FondoEncabezado { get; set; }
        public string FondoStripe { get; set; }
        public string FondoTotal { get; set; }

    }
    public class EXTablaFormato
    {
        public EXTablaFormato()
        {
            this.TipoFormato = TipoFormatoCelda.SinFormato;
        }
        public int Columna { get; set; }
        public TipoFormatoCelda TipoFormato { get; set; }
        public String Formato { get; set; }
    }

    public class EXTablaResumen
    {
        public int Pos { get; set; }
        public OpcionResumen Opcion { get; set; }
        public EXTablaFormato Formato { get; set; }
        public String Texto { get; set; }
    }

    public enum OpcionResumen
    {
        Suma = 0,
        Contador = 1,
        Maximo = 2,
        Minimo = 3,
        Nada = 4
    }

    public enum TipoFormatoCelda
    {
        SinFormato = 0,
        Numerico = 1,
        Date = 2
    }
    public enum Halinear
    {
        Izquierda = 0,
        Centro = 1,
        Derecha = 2,
        Justificado = 3,
        Automatico = 4
    }
    public enum Valinear
    {
        Abajo = 0,
        Centro = 1,
        Arriba = 2,
        Justificado = 3,
        Distribuido = 4

    }
}