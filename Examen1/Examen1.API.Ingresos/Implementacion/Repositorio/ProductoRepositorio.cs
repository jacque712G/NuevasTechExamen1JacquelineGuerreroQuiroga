using Azure.Data.Tables;
using Examen1.API.Ingresos.Contratos.Repositorio;
using Examen1.API.Ingresos.Modelo;
using Examen1.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1.API.Ingresos.Implementacion.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ProductoRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Producto";
        }
        public async Task<bool> Agregar(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(producto);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public Task<bool> Actualizar(Producto producto)
        {
            throw new NotImplementedException();
        }
       
        public Task<bool> Eliminar(string partitionkey, string rowkey)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<Producto> ObtenerById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var producto = await tablaCliente.GetEntityAsync<Producto>("Producto", rowkey);
            return producto.Value;
        }
    }
}
