using RealState.Core.Application.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.ImprovementType
{
    public class SaveImprovementTypeViewModel : CommonViewModel<int>
    {
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripcion")]
        [DataType(DataType.Text)]
        public required string Description { get; set; }

    }
}
