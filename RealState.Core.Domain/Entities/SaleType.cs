using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class SaleType : CommonEntity<int> //Tipo de venta
    {
        public required string Name { get; set; } //Nombre del tipo de venta
        public required string Description { get; set; } //Descripcion del tipo de venta

        public ICollection<PropertyUnit>? PropertyUnits { get; set; }
    }
}
