using Azure.Data.Tables;
using Examen1.API.Ingresos.Contratos.Repositorio;
using Examen1.API.Ingresos.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1.API.Ingresos.Implementacion.Repositorio
{
    public class ProveedorRepositorio : IProveedorRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ProveedorRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Proveedor";
        }
        public async Task<bool> Agregar(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(proveedor);               
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> Actualizar(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(proveedor, proveedor.ETag);
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

        public async Task<List<Proveedor>> Listar()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);

            var proveedores = tablaCliente.QueryAsync<Proveedor>(filter: "");

            await foreach (Proveedor proveedor in proveedores)
            {
                lista.Add(proveedor);
            }
            return lista;
        }
       
        public async Task<Proveedor> ObtenerById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var proveedor = await tablaCliente.GetEntityAsync<Proveedor>("Proveedor", rowkey);
            return proveedor.Value;
        }
    }
}
