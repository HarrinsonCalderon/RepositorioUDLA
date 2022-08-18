using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChairaMongo;
namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class ArchivosMongo
    {
        string tipoArchivo { get; set; }
        string nombreBaseDatos { get; set; }

        //string IP = "172.16.31.19:27017";
        string IP = "127.0.0.1:27017";
        public ArchivosMongo(string tipoArchivo = "DOC")
        {
            this.tipoArchivo = tipoArchivo;
            this.nombreBaseDatos = this.tipoArchivo.Equals("IMG") ? "DocumentosPOAI" : this.tipoArchivo.Equals("DOC") ? "DocumentosPOAI" : "";
        

        }

        public MongoInfoArchivo consultarArchivo(string idArchivoMongo)
        {
            MongoInfoArchivo archivoMongo = null;
            try
            {
                MongoNoSQL MG = new MongoNoSQL(this.nombreBaseDatos, this.IP);
                archivoMongo = MG.DocumentoConsultarId(idArchivoMongo);

            }
            catch (Exception) { }
            return archivoMongo;
        }

        public MongoRespuesta guardarArchivo(byte[] archivo, string ext, string nombreArchivo, string pege_id = "1", Dictionary<string, object> parametros = null)
        {
            MongoRespuesta respuestaArchivo = null;
            try
            {
                //MongoNoSQL MG = new MongoNoSQL(this.nombreBaseDatos, this.IP);
                MongoNoSQL MG = new MongoNoSQL("DocumentosPOAI", this.IP);
                MongoMetaData MetaDatosArchivo = new MongoMetaData();
                MetaDatosArchivo.Archivo = archivo;
                MetaDatosArchivo.Extension = ext;
                MetaDatosArchivo.NombreArchivo = nombreArchivo;
                MetaDatosArchivo.CreadoPor = pege_id;

                if (parametros != null)
                {
                    MetaDatosArchivo.Otros = parametros;
                }

                //Guardar archivo en MongoDB
                respuestaArchivo = MG.DocumentoGuardar(MetaDatosArchivo);
            }
            catch (Exception ex)
            {
                Exception ef = ex;
                return respuestaArchivo;
            }
            return respuestaArchivo;
        }

        public MongoRespuesta modificarArchivo(string idMongo, byte[] archivo, string ext, string nombreArchivo, string pege_id = "1", Dictionary<string, object> parametros = null)
        {
            MongoRespuesta respuestaArchivo = null;
            try
            {
                MongoNoSQL MG = new MongoNoSQL(this.nombreBaseDatos, this.IP);

                //Modificar archivo en MongoDB
                if (parametros == null)
                {
                    respuestaArchivo = MG.DocumentoModificarId(this.nombreBaseDatos, idMongo, nombreArchivo, pege_id, archivo);
                }
                else
                {
                    respuestaArchivo = MG.DocumentoModificarId(this.nombreBaseDatos, idMongo, nombreArchivo, pege_id, archivo, parametros);

                }
            }
            catch (Exception) { }
            return respuestaArchivo;
        }

        public MongoRespuesta eliminarArchivo(string idArchivo)
        {
            MongoRespuesta respuestaArchivo = null;
            try
            {
                MongoNoSQL MG = new MongoNoSQL(this.nombreBaseDatos, this.IP);

                //Eliminar archivo de MongoDB
                respuestaArchivo = MG.DocumentoEliminarId(idArchivo);
            }
            catch (Exception) { }
            return respuestaArchivo;
        }

    }
}