using RealState.Core.Application.DTOs.Common;
<<<<<<< HEAD
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.DTOs.ImprovementType
{
    public class ImprovementTypeDto : CommonDto<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ICollection<PropertyUnitDto>? PropertyUnits { get; set; }
=======

namespace RealState.Core.Application.DTOs.ImprovementType
{
    public class ImprovementTypeDto : CommonDto<int> 
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }
>>>>>>> 7ef5dd215724d2e6d01d1890e4b2c2f0f9e92cad

    }
}
