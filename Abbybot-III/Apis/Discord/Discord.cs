using Abbybot_III.Apis.Discord.ApiKeys;
using Abbybot_III.Apis.Discord.Events;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord
{
    class Discord
    {

        public static DiscordSocketClient _client;

        public static async void DiscordMain()
        {
            _client = new DiscordSocketClient();
            EventInitializer.Init(_client);

            await StartDiscord();

        }

        private static async Task StartDiscord()
        {
            DiscordApiKey dak = DiscordApiKey.Load(@"ApiKeys\Discord.json");
            await _client.LoginAsync(TokenType.Bot, dak.ApiKey);
            await _client.StartAsync();
            await Task.Delay(-1);
            await _client.StopAsync();
        }
    }
}
