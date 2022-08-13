using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHAIRA_GESTIONRIESGO.Modelo.Models;
using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
using MongoDB.Driver;
 
namespace CHAIRA_GESTIONRIESGO.Modelo
{
    public class MConsultar
    {
        
        private static string _cliente = "mongodb://localhost:27017";
        private static string _database = "DocumentosPOAI";
          
        #region sql server
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

        public void GuardarMenu(MongoInfoArchivo2 m, string padre) {

            try
            {
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    int ultimoMenu = bd.menu.Count() + 1;
                    menu oMenu = new menu();
                    privilegio oPrivilegio = new privilegio();
                    oMenu.extension = m.filename.Substring(m.filename.LastIndexOf(".") + 1);
                    oMenu.mid = m.Id.ToString();
                    oMenu.fkmenu = int.Parse(padre);
                    oMenu.nombre = m.filename;
                    bd.menu.Add(oMenu);
                    oPrivilegio.fkestadoprivilegio = 1;
                    oPrivilegio.fkrol = 1;
                    oPrivilegio.fkmenu =ultimoMenu;
                    bd.privilegio.Add(oPrivilegio);
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

        }
        public void GuardarCarpeta(string nombre,string padre)
        {

            try
            {
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    int ultimoMenu = bd.menu.Count() + 1;
                    menu oMenu = new menu();
                    privilegio oPrivilegio = new privilegio();
                    oMenu.extension = "Directorio";
                    oMenu.mid = "mid";
                    oMenu.fkmenu = int.Parse(padre);
                    oMenu.nombre = nombre;
                    bd.menu.Add(oMenu);
                    oPrivilegio.fkestadoprivilegio = 1;
                    oPrivilegio.fkrol = 1;
                    oPrivilegio.fkmenu = ultimoMenu;
                    bd.privilegio.Add(oPrivilegio);
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

        }
        public void ActualizarNombreCarpeta(string nombre, string Id)
        {

            try
            {
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    var tabla = bd.menu.Find(int.Parse(Id));
                    tabla.nombre = nombre;
                     
                    bd.Entry(tabla).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

        }
        #endregion


        #region mongo

        public MongoInfoArchivo2 CargarUltimoDocumento()
        {
            var cliente = new MongoClient(_cliente);
            var database = cliente.GetDatabase(_database);
            Menu documento = new Menu();
            var collection = database.GetCollection<MongoInfoArchivo2>("fs.files");
            List<MongoInfoArchivo2> result1 = (from d in collection.AsQueryable<MongoInfoArchivo2>()

                                               select new MongoInfoArchivo2
                                               {
                                                   length = d.length,
                                                   Id = d.Id,
                                                   filename = d.filename,
                                                    
                                                   
                                               }
                         ).ToList();

            return result1[result1.Count()-1];
        }
        public List<Combo> CargarComboEstado() {
            List<Combo> l=null;
            using (RepositorioUAEntities bd=new RepositorioUAEntities()) {
                l = (from a in bd.estadoprivilegio
                     select new Combo
                     {
                         Text = a.nombre,
                         Value = a.idestadoprivilegio
                     }).ToList();
            }
            return l;
        }


        #endregion

    }
}