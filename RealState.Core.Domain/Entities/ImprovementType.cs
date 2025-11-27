using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class ImprovementType : CommonEntity<int> //Tipo de mejora
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<PropertyUnit>? PropertyUnits { get; set; }
    }
}
