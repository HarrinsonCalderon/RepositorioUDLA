using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
namespace CHAIRA_GESTIONRIESGO.Modelo
{
    public class MConsultar
    {

        public List<Menu> CargarMenu(string p) {
            List<Menu> l = null;
            using (RepositorioUAEntities bd=new RepositorioUAEntities()) {
                l = (from a in bd.usuario 
                     join b in bd.rol on a.fkrol equals b.idrol
                     join c in bd.privilegio on b.idrol equals c.fkrol
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     where b.fkestadorol==1 && c.fkestadoprivilegio==1 && d.fkmenu.ToString()==p
                     select new Menu { 
                        Id=d.idmenu.ToString(),
                        Nombre=d.nombre,
                        Ruta=d.ruta,
                        Extencion=d.extension,
                        Mid=d.mid,
                        Fkmenu=d.fkmenu.ToString(),
                     }
                     ).ToList();
            }
            return l;
        }
    }
}