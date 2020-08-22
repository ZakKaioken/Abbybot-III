using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Abbybot
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.Connected += async () => await Abbybot.Connected();
            _client.Disconnected += async (exception) => await Abbybot.Disconnected(exception);
            _client.LoggedIn += async () => await Abbybot.LoggedIn();
            _client.LoggedOut += async () => await Abbybot.LoggedOut();
            _client.Ready += async () => await Abbybot.Ready();
        }

        private static Task Connected()
        {
            throw new NotImplementedException();
        }

        private static Task Disconnected(Exception exception)
        {
            throw new NotImplementedException();
        }

        private static Task LoggedIn()
        {
            throw new NotImplementedException();
        }

        private static Task LoggedOut()
        {
            throw new NotImplementedException();
        }

        private static Task Ready()
        {
            throw new NotImplementedException();
        }
    }
}