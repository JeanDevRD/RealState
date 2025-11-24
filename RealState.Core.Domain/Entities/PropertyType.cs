using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class PropertyType : CommonEntity<int> //Tipo de propiedad
    {
        public required string Name { get; set; } //Nombre del tipo de propiedad
        public required string Description { get; set; } //Descripcion del tipo de propiedad
    }
}
