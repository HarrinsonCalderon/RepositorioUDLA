using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class MongoData
    {
        //H
        public long PegeId { get; set; }
        public byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string CreadoPor { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public bool Comprimido { get; set; }
        public Dictionary<string, object> Otros { get; set; }
    }
}