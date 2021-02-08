using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGEO.DTO
{
    public class GeolocalizacionDTO
    {
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string Ciudad { get; set; }
        public int Codigo_postal { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }

    }
}
