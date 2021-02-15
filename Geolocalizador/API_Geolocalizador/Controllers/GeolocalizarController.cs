using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIGEO.DTO;
using APIGEO.Interfaces;
using APIGEO.Models;
using APIGEO.Services;
using Common.Mensajes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace MS.APIGEO.Controllers
{
    [Route("api")]
    [ApiController]
    public class GeolocalizarController : ControllerBase
    {

        private readonly ILogger<GeolocalizarController> _logger;
        private readonly IGeolocalizadorService _service;
        private readonly AmqpService _amqp;

        public GeolocalizarController(ILogger<GeolocalizarController> logger, IGeolocalizadorService service, AmqpService amqp)
        {
            _logger = logger;
            _service = service;
            this._amqp = amqp;
        }


        [Route("Geolocalizar")]
        [HttpPost]
        public ActionResult Geolocalizar(GeolocalizacionDTO body)
        {
            try
            {

                if (body == null)
                {
                    return BadRequest("Datos incorrectos.");
                }

                var id = _service.SaveGeoRequest(body);

                PeticionGeolocalizacion request = new PeticionGeolocalizacion()
                {
                    Id = id,
                    Calle = body.Calle,
                    Numero = body.Numero,
                    Ciudad = body.Ciudad,
                    Codigo_postal = body.Codigo_postal,
                    Provincia = body.Provincia,
                    Pais = body.Pais
                };

                _amqp.PublishGeolocalizacion(request);


                return Ok(request.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falló la petición de Geolocalización. Error: " + ex.Message);
            }

        }

        [Route("Geocodificar")]
        [HttpGet]
        public ActionResult Geocodificar(int id)
        {
            try
            {
                if(id == 0)
                    return BadRequest("Se debe ingresar un ID válido.");

                GeocodificacionDTO geocodificacion =  _service.GetGeocodificacionById(id);

                return Ok(geocodificacion);

            }
            catch (Exception ex)
            {
                return  StatusCode(500,ex.Message);
            }
            
        }



        //public Task UpdateGeocodificacion()
        //{

        //    RespuestaGeocodificacion rta = _amqp.ConsumeGeoCodificacion();

        //    RespuestaGeocodificacion geocodificacion = new RespuestaGeocodificacion()
        //    {
        //        Id = rta.Id,
        //        Latitud = rta.Latitud,
        //        Longitud = rta.Longitud,
        //        Estado = "Terminado"
        //    };

        //    _service.UpdateGeoRequest(geocodificacion);

            
        //}
    }
}

