using Discord.WebSocket;

namespace Abbybot_III.Apis.Events
{
    class EventInitializer
    {
        public static void Init(DiscordSocketClient _client)
        {
            Channel.Init(_client);
            User.Init(_client);
            Role.Init(_client);
            Components.Init(_client);
            Recipient.Init(_client);
            Reaction.Init(_client);
            Message.Init(_client);
            Log.Init(_client);
            Guild.Init(_client);
            AbbybotIII.Init(_client);
            //_client.
        }
    }
}