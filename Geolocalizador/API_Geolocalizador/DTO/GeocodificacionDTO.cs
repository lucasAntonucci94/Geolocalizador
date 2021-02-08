using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGEO.DTO
{
    public class GeocodificacionDTO
    {
        public string Id { get; set; }
        public int Latitud { get; set; }
        public string Longitud { get; set; }
        public int Estado { get; set; }

    }
}
