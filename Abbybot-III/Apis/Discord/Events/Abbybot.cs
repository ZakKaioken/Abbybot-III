using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Abbybot
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.Connected += async () => await Connected();
            _client.Disconnected += async (exception) => await Disconnected(exception);
            _client.LoggedIn += async () => await LoggedIn();
            _client.LoggedOut += async () => await LoggedOut();
            _client.Ready += async () => await Ready();
        }

        private static async Task Connected()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Disconnected(Exception exception)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task LoggedIn()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task LoggedOut()
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Ready()
        {
            await Task.CompletedTask;
            
            //throw new NotImplementedException();
        }
    }
}