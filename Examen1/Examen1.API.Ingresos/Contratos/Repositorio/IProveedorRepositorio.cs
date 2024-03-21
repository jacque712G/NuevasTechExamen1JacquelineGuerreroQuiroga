using Examen1.API.Ingresos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1.API.Ingresos.Contratos.Repositorio
{
    public interface IProveedorRepositorio
    {
        public Task<bool> Agregar(Proveedor proveedor);
        public Task<bool> Actualizar(Proveedor proveedor);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
        public Task<List<Proveedor>> Listar();
        public Task<Proveedor> ObtenerById(string rowkey);
    }
}
