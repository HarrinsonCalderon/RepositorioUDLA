using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Modelo.Modelsbd
{
    public class Menu
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Extencion { get; set; }
        public string Mid { get; set; }
        public string Fkmenu { get; set; }

        public int? privilegio { get; set; }
    }
}