using Microsoft.EntityFrameworkCore;

namespace Geolocalizador.Data
{ 
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Geolocalizacion> Geolocalizacion { get; set; }
		public DbSet<Geocodificacion> Geocodificacion { get; set; }


		protected override void OnConfiguring(DbContextOptionBuilder optionsBuilder)
		=> optionBuilder.UserMysql(@"Server=localhost;database=Geolocalizador;uid=guest;pwd=guest");
	}
}
