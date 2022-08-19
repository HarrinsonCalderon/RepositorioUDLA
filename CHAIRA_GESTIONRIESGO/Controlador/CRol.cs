using CHAIRA_GESTIONRIESGO.Modelo;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CRol
    {

        MRol Mr = new MRol();
         public bool AgregarRol(string nombre)
        {
            try
            {

                Mr.AgregarRol(nombre);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
        
         public List<Combo> CargarRol()
        {

            return Mr.CargarRol();

        }
           public List<Combo> CargarEstadoRol( )
        {

            return Mr.CargarEstadoRol( );

        }
        
               public Combo CargarEstadoRolId(string id)
        {

            return Mr.CargarEstadoRolId(id);

        }
       
              public bool CambiarEstado(string idmenu, string idrol)
        {
            try
            {

                Mr.CambiarEstado(idmenu,idrol);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
        
       
    }
}