using CHAIRA_GESTIONRIESGO.Conexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Modelo
{
    public class MCorreos
    {
        DBOracle conexion = new DBOracle();

        public DataTable FN_VINCULACIONPORTIPOUSUARIO(string tipousuario)
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("v_TIPOUSUARIO", tipousuario, "VARCHAR2", ParameterDirection.Input));
            parametros.Add(new Parametro("DATOS", "", "CURSOR", ParameterDirection.ReturnValue));
            return conexion.OraProcedimiento("ADMINISTRATIVOOAD.FN_VINCULACIONPORTIPOUSUARIO", parametros, "admin");
        }

        public DataTable FN_TIPOUSUARIO()
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("DATOS", "", "CURSOR", ParameterDirection.ReturnValue));
            return conexion.OraProcedimiento("ADMINISTRATIVOOAD.FN_TIPOUSUARIO", parametros, "admin");
        }

        public DataTable FN_EMPTIPOUSUARIOVINCULACION(string tipousuario, string vinculacion)
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("v_TIPOUSUARIO", tipousuario, "VARCHAR2", ParameterDirection.Input));
            parametros.Add(new Parametro("v_VINCULACION", vinculacion, "VARCHAR2", ParameterDirection.Input));
            parametros.Add(new Parametro("DATOS", "", "CURSOR", ParameterDirection.ReturnValue));
            return conexion.OraProcedimiento("ADMINISTRATIVOOAD.FN_EMPTIPOUSUARIOVINCULACION", parametros, "admin");
        }

        public DataTable FN_BUSCARPERSONA(string parametro)
        {
            List<Parametro> parametros = new List<Parametro>();

            parametros.Add(new Parametro("PARAMETRO", parametro, "VARCHAR2", ParameterDirection.Input));
            parametros.Add(new Parametro("DATOS", "", "CURSOR", ParameterDirection.ReturnValue));

            return conexion.OraProcedimiento("ADMINISTRATIVOOAD.FN_BUSCARPERSONA", parametros, "admin");
        }

        public DataTable FN_CREDENCIALES()
        {
            List<Parametro> parametros = new List<Parametro>();

            parametros.Add(new Parametro("DATOS", "", "CURSOR", ParameterDirection.ReturnValue));

            return conexion.OraProcedimiento("ADMINISTRATIVOOAD.FN_CREDENCIALES", parametros, "admin");
        }

    }
}