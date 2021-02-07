using Abbybot_III.Core.Heart;

using System;
using System.Collections.Generic;
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
            foreach (RequestObject requestObject in requestObjects)
                if (requestObject.time < time)
                {
                    switch (requestObject.requestType)
                    {
                        case RequestType.Delete:
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
                            break;

                        case RequestType.Reminder:
                            await requestObject.itc.SendMessageAsync(requestObject.o.ToString());
                            break;

                        default:
                            break;
                    }
                    RemoveRequest(requestObject);
                }
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