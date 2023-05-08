using Backbone.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Chat
{
    public static class ChatManager<T>
    {
        public static Dictionary<string, T> ActionTypeMapping = new Dictionary<string, T>();

        public static async void InitializeChat(TCPChatSettings settings)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(settings.IP, settings.Port);

            var streamReader = new StreamReader(tcpClient.GetStream());
            var streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

            await streamWriter.WriteLineAsync($"PASS {settings.Password}");
            await streamWriter.WriteLineAsync($"NICK {settings.Nick}");

            //await streamWriter.WriteLineAsync("PRIVMSG #cableshaft :This is a test message");

            while (true)
            {
                string message = await streamReader.ReadLineAsync();
                if (message != null && message.Contains("PRIVMSG"))
                {
                    ParseTwitchChat(message);
                }
            }
        }

        public static async void ParseTwitchChat(string message)
        {
            // Get the users name by splitting it from the string

            var split = message.IndexOf("!", 1);
            var audienceMember = message.Substring(0, split);
            audienceMember = audienceMember.Substring(1);

            split = message.IndexOf(":", 1);
            message = message.Substring(split + 1);

            var actionSplit = message.Split(" ");
            var action = actionSplit[0];

            // only fire an event if it matches something int he mapping
            if(ActionTypeMapping.ContainsKey(action))
            {
                var actionType = ActionTypeMapping[action];
                var chatAction = new ChatAction<T>()
                {
                    AudienceMember = audienceMember,
                    SelectedAction = actionType,
                    Message = message
                };

                PubHub<T>.RaiseAsync(actionType, chatAction);
            }
        }
    }
}
