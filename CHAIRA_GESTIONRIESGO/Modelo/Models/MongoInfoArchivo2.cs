using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CHAIRA_GESTIONRIESGO.Modelo.Models
{
    public class MongoInfoArchivo2
    {
         
        public object Id { get; set; }
        //[BsonElement("creadopor")]
        public string CreadoPor { get; set; }
        public string PegeId { get; set; }
        public byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Md5 { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public Dictionary<string, object> Otros { get; set; }
        public bool Comprimido { get; set; }
        public string Extension { get; set; }
        public long Tamano { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
        public string MensajeRespuesta { get; set; }

        //fs.files
        public int length { get; set; }
        public int chunkSize { get; set; }
        public string md5 { get; set; }
        public string filename { get; set; }
        Object metadata { get; set; }

        //fs.chunks
        public byte[] data { get; set; }
        public object files_id { get; set; }
    }

    public class metadata2 {
        public string PegeId { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string CreadoPor { get; set; }
        public string Ancho { get; set; }
        public string Alto { get; set; }
        public string Comprimido { get; set; }
    }
}