using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Chat
{
    public class TCPChatSettings
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

        // username for the bot user (for Twitch) or just the nickname for the IRC chat
        public string Nick { get; set; }
    }
}
