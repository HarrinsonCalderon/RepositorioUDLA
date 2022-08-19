using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
using CHAIRA_GESTIONRIESGO.Modelo;
using CHAIRA_GESTIONRIESGO.Modelo.Models;

namespace CHAIRA_GESTIONRIESGO.Controlador
{
    public class CConsultar
    {
        MConsultar Mc = new MConsultar();
        public List<Menu> CargarMenu(string p,string idrol)
        {
            return Mc.CargarMenu(p, idrol);
        }
        public List<Menu> CargarMenuPublico(string p, string idrol)
        {
            return Mc.CargarMenuPublico(p, idrol);
        }
        public List<Menu> CargarMenuCambioRol(string p, string idrol)
        {
            return Mc.CargarMenuCambioRol(p, idrol);
        }
        public List<Menu> CargarMenu2(string p, string idrol)
        {
            return Mc.CargarMenu2(p, idrol);
        }
        public MongoInfoArchivo2 CargarUltimoDocumento()
        {
            return Mc.CargarUltimoDocumento();
        }
        public bool GuardarMenu(MongoInfoArchivo2 m,string padre) {
            try {
                    
                Mc.GuardarMenu(m,padre);
                return true;
            }
            catch (Exception ea) {
                return false;
            }
        }
        public bool GuardarCarpeta(string nombre, string padre) {
            try
            {

                Mc.GuardarCarpeta(nombre, padre);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
        public bool ActualizarNombreCarpeta(string nombre, string Id) {
            try
            {

                Mc.ActualizarNombreCarpeta(nombre, Id);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
        public List<Combo> CargarComboEstado()
        {
            
               return Mc.CargarComboEstado();
 
        }
        public Combo CargarFormularioCarpeta(string id) {
            return Mc.CargarFormularioCarpeta(id);
        }
        public bool ActualizarEstadoCarpeta(string Id, string EstadoNuevo) {
            try
            {

                Mc.ActualizarEstadoCarpeta( Id,  EstadoNuevo);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
        public bool CambiarNombre(string idmenu, string nombre)
        {
            try
            {

                Mc.CambiarNombre(idmenu, nombre);
                return true;
            }
            catch (Exception ea)
            {
                return false;
            }
        }
    }
}