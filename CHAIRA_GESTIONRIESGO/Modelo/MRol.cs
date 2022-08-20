using CHAIRA_GESTIONRIESGO.Modelo.Modelsbd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Modelo
{
    public class MRol
    {

        public void AgregarRol(string nombre)
        {
            try {
                using (RepositorioUAEntities bd=new RepositorioUAEntities()) {
                    rol oRol = new rol();
                    oRol.nombre = nombre;
                    oRol.fkestadorol = 1;
                    bd.rol.Add(oRol);
                    bd.SaveChanges();
                }
            }
            catch (Exception ea) { 
            
            }
        
        }
        public List<Combo> CargarRol()
        {
            List<Combo> l = null;
            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = (from a in bd.rol
                     where a.idrol!=1
                     select new Combo
                     {
                         Text = a.nombre,
                         Value = a.idrol
                     }).ToList();
            }
            return l;
        }
        public List<Combo> CargarEstadoRol(  )
        {
            List<Combo> l = null;
            using (RepositorioUAEntities bd = new RepositorioUAEntities())
            {
                l = (from a in bd.estadorol
                     select new Combo
                     {
                         Text = a.nombre,
                         Value = a.idestadorol
                     }).ToList();     
                }          
            return l;
        }
        public  Combo CargarEstadoRolId(string id)
        {
            Combo l = null;
            try
            {
                int Id = int.Parse(id);
                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    l = (from a in bd.estadorol
                         join b in bd.rol
                         on a.idestadorol equals b.fkestadorol
                         where b.idrol == Id 
                         select new Combo
                         {
                             Text = a.nombre,
                             Value = a.idestadorol
                         }).FirstOrDefault();
                }
            }
            catch (Exception ea) { 
            
            }
            
            return l;
        }
        public Combo CambiarEstado(string idmenu, string idrol)
        {
            Combo l = null;
            try
            {
                int Idmenu = int.Parse(idmenu);
                int Idrol = int.Parse(idrol);

                using (RepositorioUAEntities bd = new RepositorioUAEntities())
                {
                    l = (from a in bd.privilegio
                         
                         where a.fkrol == Idrol && a.fkmenu==Idmenu
                         select new Combo
                         {
                             
                             Value = a.idprivilegio,
                             Value2=a.fkestadoprivilegio
                         }).FirstOrDefault();
                    var Oprivilegio = bd.privilegio.Find(l.Value);
                    if (l.Value2 == 1)
                        Oprivilegio.fkestadoprivilegio = 2;
                    else
                        Oprivilegio.fkestadoprivilegio = 1;
                    bd.Entry(Oprivilegio).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ea)
            {

            }

            return l;
        }
       
    }
    }
 