using Examen1.API.Ingresos.Contratos.Repositorio;
using Examen1.API.Ingresos.Modelo;
using Examen1.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Examen1.API.Ingresos.EndPoints
{
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedorRepositorio repos;

        public ProveedorFunction(ILogger<ProveedorFunction> logger,IProveedorRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("AgregarProveedor")]
        [OpenApiOperation("Agregarspec", "AgregarProveedor", Description = "Sirve para Agregar un Proveedor")]
        [OpenApiSecurity("passw0rd", SecuritySchemeType.ApiKey, Name = "Seguridad", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Mostrara el Proveedor Creado")]
        public async Task<HttpResponseData> AgregarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AgregarProveedor")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar todos los datos de un Proveedor");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool seGuardo = await repos.Agregar(registro);
                if (seGuardo)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    var proveedor = repos.ObtenerById(registro.RowKey);
                    await respuesta.WriteAsJsonAsync(proveedor.Result);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ObtenerProveedorById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProveedorById", Description = "Sirve para obtener un Proveedor")]
        [OpenApiSecurity("passw0rd", SecuritySchemeType.ApiKey, Name = "Seguridad", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Mostrara un Proveedor")]
        public async Task<HttpResponseData> ObtenerProveedorById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerProveedorById/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var proveedor = repos.ObtenerById(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(proveedor.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
       
    }
}
