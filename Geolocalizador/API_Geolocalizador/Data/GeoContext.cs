using APIGEO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGEO.Data
{
    public class GeoContext : DbContext
    {
        public GeoContext (DbContextOptions<GeoContext> options) : base(options)
        { 
        }

        public DbSet<GeolocalizacionDB> Geolocalizacion { get; set; }
    }
}
