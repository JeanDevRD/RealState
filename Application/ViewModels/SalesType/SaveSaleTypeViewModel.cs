using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.SalesType
{
    public class SaveSaleTypeViewModel : CommonViewModel<int> 
    {
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripcion")]
        [DataType(DataType.Text)]
        public required string Description { get; set; }
        public int? CountProperty { get; set; } = 0;

    }
}
