using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Modelo.Models
{
    public class metadata
    {
        public object Id { get; set; }
        public string PegeId { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string CreadoPor { get; set; }
        public string Ancho { get; set; }
        public string Alto { get; set; }
        public string Comprimido { get; set; }
    }
}