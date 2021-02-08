using APIGEO.Data;
using APIGEO.DTO;
using APIGEO.Interfaces;
using APIGEO.Models;
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


            return 1;
        }

        public void PublishGeolocalizacion(PeticionGeolocalizacion request)
        {

            //guardar el pedido de geolocalizacion en DB utilizando pomelo

            //SE ENVIAN LOS DATOS AL A COLA CORRESPONDIENTE

            //var factory = new ConnectionFactory() { HostName = "localhost"};
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "adwadwad",
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    string message = "Hello World!";
            //    var respose = Encoding.UTF8.GetBytes(message);

            //    channel.BasicPublish(exchange: "",
            //                         routingKey: "hello",
            //                         basicProperties: null,
            //                         body: respose);
            //    Console.WriteLine(" [x] Sent {0}", message);
            //}

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();

            //servicio.Publish(dato);  - Al publicar deberia obtener un id para devolver.
        }

    }
}
