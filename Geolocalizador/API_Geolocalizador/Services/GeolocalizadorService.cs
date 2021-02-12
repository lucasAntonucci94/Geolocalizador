using APIGEO.Data;
using APIGEO.DTO;
using APIGEO.Interfaces;
using APIGEO.Models;
using Common.Mensajes;
using Microsoft.Extensions.Logging;
using MS.APIGEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGEO.Services
{
    public class GeolocalizadorService : IGeolocalizadorService
    {


        public GeoContext _db { get; set; }

        public GeolocalizadorService(GeoContext db)
        {
            _db = db;
        }

        public int SaveGeoRequest(GeolocalizacionDTO body) 
        {

            GeolocalizacionDB geolocalizacion = new GeolocalizacionDB()
            {
                Calle = body.Calle,
                Numero = body.Numero,
                Ciudad = body.Ciudad,
                Codigo_postal = body.Codigo_postal,
                Provincia = body.Provincia,
                Pais = body.Pais,
                Latitud = null,
                Longitud = null,
                Estado = "Procesando"
            };

            _db.Geolocalizacion.Add(geolocalizacion);
            _db.SaveChanges();

            return geolocalizacion.Id;

        }

        public GeocodificacionDTO GetGeocodificacionById(int id) 
        {

            GeolocalizacionDB dbGeo = _db.Geolocalizacion
                .Where(x => x.Id == id)
                .FirstOrDefault();

            GeocodificacionDTO geocodificacion = new GeocodificacionDTO()
            {
                Id = dbGeo.Id,
                Latitud = dbGeo.Latitud,
                Longitud = dbGeo.Latitud,
                Estado = dbGeo.Estado
            };

            return geocodificacion;
   
        }
    }
}
