using CHAIRA_GESTIONRIESGO.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CCorreos
    {
        MCorreos _MCorreos = new MCorreos();

        public DataTable FN_VINCULACIONPORTIPOUSUARIO(string tipousuario)
        {
            return _MCorreos.FN_VINCULACIONPORTIPOUSUARIO(tipousuario);
        }

        public DataTable FN_TIPOUSUARIO()
        {
            return _MCorreos.FN_TIPOUSUARIO();
        }

        public DataTable FN_EMPTIPOUSUARIOVINCULACION(string tipousuario, string vinculacion)
        {
            return _MCorreos.FN_EMPTIPOUSUARIOVINCULACION(tipousuario, vinculacion);
        }

        public DataTable FN_BUSCARPERSONA(string parametro)
        {
            return _MCorreos.FN_BUSCARPERSONA(parametro);
        }

        public DataTable FN_CREDENCIALES()
        {
            return _MCorreos.FN_CREDENCIALES();
        }

    }
}