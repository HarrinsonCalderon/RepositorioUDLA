using CHAIRA_GESTIONRIESGO.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CEjemplo
    {
        MEjemplo Rpor = new MEjemplo();


        #region Ejemplo

        public DataTable FN_REPORTESPDINUEVO(string VAR_OPC)
        {
            return Rpor.FN_REPORTESPDINUEVO(VAR_OPC);
        }


        #endregion
    }
}