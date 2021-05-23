using Abbybot_III.Apis.ApiKeys;
using Abbybot_III.Apis.Events;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Apis
{
    class Discord
    {
        public static DiscordSocketClient _client;

        static async Task StartDiscord()
        {
            bool o = true;
            do
            {
                try
                {
                    DiscordApiKey dak = DiscordApiKey.Load(@"ApiKeys\Discord.json");
                    await _client.LoginAsync(TokenType.Bot, dak.ApiKey);
                    await _client.StartAsync();
                    o = false;
                }
                catch
                {
                    Abbybot.print("Failed to start discord. Trying again in 10 seconds.");
                    await Task.Delay(10000);
                }
            } while (o);
        }

        public static async Task IndefinitelyWaitUntilClose()
        {
            await Task.Delay(-1);
            await _client.StopAsync();
        }

        public static async Task DiscordMainAsync()
        {
            _client = new DiscordSocketClient();
            EventInitializer.Init(_client);

            await StartDiscord();
        }
    }
}