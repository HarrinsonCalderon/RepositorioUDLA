using CHAIRA_GESTIONRIESGO.Conexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Modelo
{
    public class MEjemplo
    {
        DBOracle conexion = new DBOracle();


        #region Ejemplo

        public DataTable FN_REPORTESPDINUEVO(string VAR_OPC)
        {
            DataSet data = new DataSet();

            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("VAR_OPC", VAR_OPC, "NUMBER", ParameterDirection.Input));
            parametros.Add(new Parametro("DATOSPROC", "", "CURSOR", ParameterDirection.ReturnValue));
            //return conexion.OraProcedimiento("NOMBRE_ESQUEMA.FN_REPORTESPDINUEVO", parametros);
            return conexion.OraProcedimiento("POAIOAD.FN_REPORTESPDINUEVO", parametros);

        }
        #endregion
    }
}