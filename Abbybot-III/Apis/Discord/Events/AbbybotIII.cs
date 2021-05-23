using Abbybot_III.Clocks;

using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Events
{
    public class AbbybotIII
    {
        public static void Init(DiscordSocketClient _client)
        {
            _client.Connected += async () => await Connected();
            _client.Disconnected += async (exception) => await Disconnected(exception);
            _client.LoggedIn += async () => await LoggedIn();
            _client.LoggedOut += async () => await LoggedOut();
            _client.Ready += async () => await Ready();
        }

        static async Task Connected()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Disconnected(Exception exception)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task LoggedIn()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task LoggedOut()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Ready()
        {
            await Task.CompletedTask;

            await ClockIniter.init();
        }
    }
}