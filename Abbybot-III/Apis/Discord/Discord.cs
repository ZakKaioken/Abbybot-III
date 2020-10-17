using Abbybot_III.Apis.Discord.ApiKeys;
using Abbybot_III.Apis.Discord.Events;

using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord
{
    class Discord
    {

        public static DiscordSocketClient _client;


        private static async Task StartDiscord()
        {
            bool o = true;
            do
            {
                try {
                DiscordApiKey dak = DiscordApiKey.Load(@"ApiKeys\Discord.json");
                await _client.LoginAsync(TokenType.Bot, dak.ApiKey);
                await _client.StartAsync();
                    o = false;
                } catch
                {
                    Console.WriteLine("Failed to start discord. Trying again in 10 seconds.");
                    await Task.Delay(10000);
                }
            } while (o);
        }

        internal static async Task IndefinitelyWaitUntilClose()
        {
            await Task.Delay(-1);
            await _client.StopAsync();
        }

        internal static async Task DiscordMainAsync()
        {
            _client = new DiscordSocketClient();
            EventInitializer.Init(_client);

            await StartDiscord();
        }
    }
}
