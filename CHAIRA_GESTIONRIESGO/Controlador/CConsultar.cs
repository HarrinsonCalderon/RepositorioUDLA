using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
using CHAIRA_GESTIONRIESGO.Modelo;
namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CConsultar
    {
        MConsultar Mc = new MConsultar();
        public List<Menu> CargarMenu()
        {
            return Mc.CargarMenu();
        }
    }
}