using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class Property : CommonEntity<int> //Propiedad
    {
        public required string IdAgent { get; set; } //IdAgente

        public required int PropertyTypeId { get; set; } //TipoPropiedad
        public PropertyType? PropertyType { get; set; }

        public required int SaleTypeId { get; set; } //TipoDeVenta
        public SaleType? SaleType { get; set; }

        public required List<ImprovementType> ImprovementTypes { get; set; } //TiposDeMejoras

        public required string CodeProperty { get; set; } //CodigoDePropiedad

        public required decimal Price { get; set; } //Precio
        public required string Description { get; set; } //Descripcion
        public required double SizeM { get; set; } //Tamaño en metros
        public required int Bedrooms { get; set; } //Habitaciones
        public required int Bathrooms { get; set; } //Baños
        public required List<string> Images { get; set; } //Imagenes
        public required int StateProperty { get; set; } //Estado de la propiedad

    }
}
