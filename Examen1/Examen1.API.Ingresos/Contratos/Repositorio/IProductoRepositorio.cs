using Examen1.API.Ingresos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1.API.Ingresos.Contratos.Repositorio
{
    public interface IProductoRepositorio
    {
        public Task<bool> Agregar(Producto producto);
        public Task<bool> Actualizar(Producto producto);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
        public Task<List<Producto>> Listar();
        public Task<Producto> ObtenerById(string rowkey);
    }
}
