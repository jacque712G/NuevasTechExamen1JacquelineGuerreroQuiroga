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
        public async Task<bool> Actualizar(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(producto, producto.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
       
        public async Task<bool> Eliminar(string partitionkey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Producto>> Listar()
        {
            List<Producto> lista = new List<Producto>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);

            var productos = tablaCliente.QueryAsync<Producto>(filter: "");

            await foreach (Producto producto in productos)
            {
                lista.Add(producto);
            }
            return lista;
        }

        public async Task<Producto> ObtenerById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var producto = await tablaCliente.GetEntityAsync<Producto>("Producto", rowkey);
            return producto.Value;
        }
    }
}
