using APIGEO.DTO;
using Common.Mensajes;
using MS.APIGEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGEO.Interfaces
{
    public interface IGeolocalizadorService
    {
        int SaveGeoRequest(GeolocalizacionDTO body);
        void UpdateGeoRequest(GeolocalizacionDTO body);
        GeocodificacionDTO GetGeocodificacionById(int id);
    }
}
