using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;

namespace Abbybot_III.Apis.Discord.Events
{
    class EventInitializer
    {
        public static void Init(DiscordSocketClient _client)
        {
            Channel.Init(_client);
            User.Init(_client);
            Role.Init(_client);
            Recipient.Init(_client);
            Reaction.Init(_client);
            Message.Init(_client);
            Log.Init(_client);
            Guild.Init(_client);
            Abbybot.Init(_client);

        }
    }
}
