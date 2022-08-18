using CHAIRA_GESTIONRIESGO.Conexion;
using CHAIRA_GESTIONRIESGO.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CUtil
    {

        public static string INSERTARAUDAPLICATIVO(List<Parametro> obj)
        {
            CProcedimientos insertar = new CProcedimientos();
            string resultado = insertar.EjecutarOperacion("POAOAD.PR_INSERTARAUDAPLICATIVO", obj);
            return resultado;
        }

        public static DataTable getEstadoBD()
        {
            List<Parametro> obj = new List<Parametro>
            {
                new Parametro("AUDISERVIDOR", "" , "CURSOR", ParameterDirection.ReturnValue)
            };

            CProcedimientos dataload = new CProcedimientos();
            return dataload.EjecutarSelect("POAOAD.FN_CONSULTARAUDAPPBD", obj);
        }

        public static DataTable getEstadoServidor()
        {
            List<Parametro> obj = new List<Parametro>
            {
                new Parametro("AUDIBD", "" , "CURSOR", ParameterDirection.ReturnValue)
            };

            CProcedimientos dataload = new CProcedimientos();
            return dataload.EjecutarSelect("POAOAD.FN_CONSULTARAUDAPPSERVIDOR", obj);
        }
    }
}