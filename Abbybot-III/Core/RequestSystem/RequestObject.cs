using Discord.Rest;
using Discord.WebSocket;

using System;

namespace Abbybot_III.Core.RequestSystem
{
    public class RequestObject
    {
        public DateTime time;
        public ISocketMessageChannel itc;
        public RestUserMessage AbbybotMessage;
        public SocketMessage UsersMessage;
        public object o;
        public RequestType requestType;
    }

    public enum RequestType
    {
        Unknown = 0,
        Delete = 1,
        Reminder = 2
    }
}