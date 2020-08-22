using Abbybot_III.Apis.Discord.ApiKeys;
using Abbybot_III.Apis.Discord.Events;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord
{
    class Discord
    {
        public static DiscordSocketClient _client;
        public void DiscordMain()
        {
            _client = new DiscordSocketClient();
            EventInitializer.Init(_client);

            StartDiscord().GetAwaiter().GetResult();
            
        }

        private async Task StartDiscord()
        {
            DiscordApiKey dak = DiscordApiKey.Load("DiscordApiKey.json");
            await _client.LoginAsync(TokenType.Bot, dak.ApiKey);
            await _client.StartAsync();
            await Task.Delay(-1);
            await _client.StopAsync();
        }
    }
}
