using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.UserFavoritePropertyUnit
{
    public class UserFavoritePropertyUnitViewModel
    {
        public required int Id { get; set; }
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
    }
}
