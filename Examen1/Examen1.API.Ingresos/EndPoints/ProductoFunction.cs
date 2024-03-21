using Examen1.API.Ingresos.Contratos.Repositorio;
using Examen1.API.Ingresos.Modelo;
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
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoRepositorio repos;

        public ProductoFunction(ILogger<ProductoFunction> logger,IProductoRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("AgregarProducto")]
        [OpenApiOperation("Agregarspec", "AgregarProducto", Description = "Sirve para Agregar un Producto")]
        [OpenApiSecurity("passw0rd", SecuritySchemeType.ApiKey, Name = "Seguridad", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(Producto), Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Mostrara el Producto Creado")]
        public async Task<HttpResponseData> AgregarProducto([HttpTrigger(AuthorizationLevel.Function, "post", Route = "AgregarProducto")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar todos los datos de un Producto");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool seGuardo = await repos.Agregar(registro);
                if (seGuardo)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    var producto = repos.ObtenerById(registro.RowKey);
                    await respuesta.WriteAsJsonAsync(producto.Result);
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
        [Function("ObtenerProductoById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProductoById", Description = "Sirve para obtener un Producto")]
        [OpenApiSecurity("passw0rd", SecuritySchemeType.ApiKey, Name = "Seguridad", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Mostrara un Producto")]
        public async Task<HttpResponseData> ObtenerProductoById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "ObtenerProductoById/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var producto = repos.ObtenerById(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(producto.Result);
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
