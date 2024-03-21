using Azure;
using Azure.Data.Tables;
using Examen1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1.API.Ingresos.Modelo
{
    public class Proveedor : IProveedor, ITableEntity
    {
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get ; set ; } = null!;
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get ; set ; }
    }
}
