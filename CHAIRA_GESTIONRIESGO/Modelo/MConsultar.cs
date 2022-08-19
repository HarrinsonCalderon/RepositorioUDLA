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
        public List<Menu> CargarMenu2(string p, string idrol)
        {
            List<Menu> l = null;
            int Idrol = 0;
            if (idrol != "")
                Idrol = int.Parse(idrol);

            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = (from a in bd.usuario
                     join b in bd.rol on a.fkrol equals b.idrol
                     join c in bd.privilegio on b.idrol equals c.fkrol
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     //where b.fkestadorol==1 && c.fkestadoprivilegio==1 && d.fkmenu.ToString()==p
                     where c.fkestadoprivilegio==1 && d.fkmenu.ToString() == p && c.fkrol == Idrol
                     select new Menu
                     {
                         Id = d.idmenu.ToString(),
                         Nombre = d.nombre,
                          
                         Extencion = d.extension,
                         Mid = d.mid,
                         Fkmenu = d.fkmenu.ToString(),
                         privilegio = c.fkestadoprivilegio
                     }
                     ).ToList();
            }
            return l;
        }
        public List<Menu> CargarMenu(string p, string idrol) {
            List<Menu> l = null;
            int Idrol=1 ;
            int Idmenu=1 ;
            if(idrol!="")
              Idrol = int.Parse(idrol);
            if (p != "")
                Idmenu = int.Parse(p);
            
            using (RepositorioUAEntities bd=new RepositorioUAEntities()) {
                l = (from a in bd.usuario 
                     join b in bd.rol on a.fkrol equals b.idrol
                     join c in bd.privilegio on b.idrol equals c.fkrol
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     where d.fkmenu == Idmenu && b.idrol == Idrol
                     
                     select new Menu { 
                        Id=d.idmenu.ToString(),
                        Nombre=d.nombre,
                        Extencion=d.extension,
                        Mid=d.mid,
                        Fkmenu=d.fkmenu.ToString(),
                        privilegio=c.fkestadoprivilegio
                     }
                     ).ToList();
            }
            return l;
        }
        public List<Menu> CargarMenu2(string p)
        {
            List<Menu> l = null;
            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = (from a in bd.usuario
                     join b in bd.rol on a.fkrol equals b.idrol
                     join c in bd.privilegio on b.idrol equals c.fkrol
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     where b.fkestadorol==1 && c.fkestadoprivilegio==1 && d.fkmenu.ToString()==p
                     
                     select new Menu
                     {
                         Id = d.idmenu.ToString(),
                         Nombre = d.nombre,
                          
                         Extencion = d.extension,
                         Mid = d.mid,
                         Fkmenu = d.fkmenu.ToString(),
                     }
                     ).ToList();
            }
            return l;
        }
        public List<Menu> CargarMenuPublico(string p, string idrol)
        {
            List<Menu> l = null;
            int Idrol = 1;
            int Idmenu = 1;
            if (idrol != "")
                Idrol = int.Parse(idrol);
            if (p != "")
                Idmenu = int.Parse(p);

            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = ( 
                     from c in bd.privilegio
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     where c.fkestadoprivilegio == 1 && d.fkmenu.ToString() == p && c.fkrol==3

                     select new Menu
                     {
                         Id = d.idmenu.ToString(),
                         Nombre = d.nombre,
                         Extencion = d.extension,
                         Mid = d.mid,
                         Fkmenu = d.fkmenu.ToString(),
                         privilegio = c.fkestadoprivilegio
                     }
                     ).ToList();
            }
            return l;
        }
        public List<Menu> CargarMenuCambioRol(string p, string idrol)
        {
            List<Menu> l = null;
            int Idrol = 1;
            int Idmenu = 1;
            if (idrol != "")
                Idrol = int.Parse(idrol);
            if (p != "")
                Idmenu = int.Parse(p);

            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = (
                     from c in bd.privilegio
                     join d in bd.menu on c.fkmenu equals d.idmenu
                     where   d.fkmenu.ToString() == p && c.fkrol == Idrol

                     select new Menu
                     {
                         Id = d.idmenu.ToString(),
                         Nombre = d.nombre,
                         Extencion = d.extension,
                         Mid = d.mid,
                         Fkmenu = d.fkmenu.ToString(),
                         privilegio = c.fkestadoprivilegio
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
        public void ActualizarEstadoCarpeta(string Id, string EstadoNuevo)
        {

            try
            {
                int idp = int.Parse(Id);
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    privilegio oPrivilegio = new privilegio();
                    oPrivilegio = (from a in bd.privilegio
                                where a.fkmenu == idp
                                select   a
                                 
                                ).FirstOrDefault();
                    oPrivilegio.fkestadoprivilegio = int.Parse(EstadoNuevo);
                    bd.Entry(oPrivilegio).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

        }
        public Combo CargarFormularioCarpeta(string id)
        {

            try
            {
                int Id = int.Parse(id);
                Combo datos = null; 
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    datos = (from a in bd.menu
                             join b in bd.privilegio
                             on a.idmenu equals b.fkmenu
                             where a.idmenu == Id
                             select new Combo
                             {
                                 Text = a.nombre,
                                 Value2 = b.fkestadoprivilegio,
                                 Value3=a.mid,
                                 Id=a.idmenu
                             }).FirstOrDefault();
                    return datos;
                }
            }
            catch (Exception ea)
            {
                return new Combo();
            }

        }
        public Combo CambiarNombre(string idmenu, string nombre)
        {
            Combo l = null;
            try
            {

                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    l = (from a in bd.menu

                         where a.mid == idmenu
                         select new Combo
                         {
                             Value = a.idmenu,
                         }).FirstOrDefault();
                    var Omenu = bd.menu.Find(l.Value);
                    Omenu.nombre = nombre;
                    bd.Entry(Omenu).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

            return l;
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