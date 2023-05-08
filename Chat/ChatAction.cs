using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Chat
{
    public class ChatAction<T>
    {
        public string Message { get; set; } = string.Empty;
        public string AudienceMember { get; set; } = string.Empty;

        public T SelectedAction { get; set; }
    }
}
