using System;
using System.Net.Http;
using System.Text;
using APIGEO.DTO;
using APIGEO.Interfaces;
using APIGEO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace MS.APIGEO.Controllers
{
    [ApiController]
    [Route("[controller]")]
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



        [HttpPost]
        public ActionResult Geolocalizar(GeolocalizacionDTO body)
       {


            if (body == null) {
                return BadRequest();
            }

            var id = _service.SaveGeoRequest(body);

            PeticionGeolocalizacion request = new PeticionGeolocalizacion() {
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


        [HttpGet]
        public string GetGeo(string id)
        {
            //return "APIGEO";
            id = (id == null) ? string.Empty : id;



            return "id: " + id;
        }
        //actualiza db tras respuesta del servicio de geocodificacion -metodo aparte
    }
}

