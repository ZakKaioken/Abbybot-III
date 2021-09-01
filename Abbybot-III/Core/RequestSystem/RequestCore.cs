using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Heart;

using Discord.Rest;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Core.RequestSystem
{
    class RequestCore
    {
        static List<RequestObject> requestObjects = new List<RequestObject>();

        public static void Init()
        {
            AbbyHeart.heartBeat += async (time) => await RequestBeat(time);
        }

        static async Task RequestBeat(DateTime time)
        {
            foreach (RequestObject requestObject in requestObjects.ToList())
                if (requestObject.time < time)
                {
                    await (requestObject.requestType switch {
                        RequestType.Delete => DeleteRequest(requestObject),
                        RequestType.Reminder => requestObject.itc.SendMessageAsync(requestObject.o.ToString()),
                        _ => Task.CompletedTask
                    }); 
                    RemoveRequest(requestObject);
                }
        }

		private static async Task DeleteRequest(RequestObject requestObject)
		{
			try
			{
				await requestObject.UsersMessage.DeleteAsync();
			}
			catch { }
			try
			{
				await requestObject.AbbybotMessage.DeleteAsync();
			}
			catch { }
		}
        public static void AddRequest(RestUserMessage abbybot, SocketMessage user, RequestType type, DateTime time, ISocketMessageChannel imc =null)
        {
            var rq = new RequestObject() { AbbybotMessage = abbybot, UsersMessage = user, itc = imc, requestType = type, time = time };
            requestObjects.Add(rq);
        }
        public static void AddRequest(RequestObject requestObject)
        {
            requestObjects.Add(requestObject);
        }

        public static void RemoveRequest(RequestObject requestObject)
        {
            requestObjects.Remove(requestObject);
        }
    }
}