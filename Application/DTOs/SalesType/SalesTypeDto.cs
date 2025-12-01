using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Entities;


namespace RealState.Core.Application.DTOs.SalesType
{
    public class SalesTypeDto
    {
        public required string Name { get; set; } 
        public required string Description { get; set; } 

        public ICollection<PropertyUnitDto>? PropertyUnits { get; set; }
    }
}
