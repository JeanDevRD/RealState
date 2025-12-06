using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.Chat
{
    public class ChatListItemViewModel
    {
        public int ChatId { get; set; }
        public string ClientId { get; set; } = "";
        public string ClientName { get; set; } = "";
        public int PropertyId { get; set; }
    }
}
