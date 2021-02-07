using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    public class Guild
    {
        public static void Init(DiscordSocketClient _client)
        {
            _client.JoinedGuild += async (guild) => await Guild.Joined(guild);
            _client.LeftGuild += async (guild) => await Guild.Left(guild);
            _client.GuildUpdated += async (oldguild, newguild) => await Guild.Updated(oldguild, newguild);
            _client.GuildAvailable += async (guild) => await Guild.Available(guild);
            _client.GuildUnavailable += async (guild) => await Guild.Unavailable(guild);
        }

        static async Task Joined(SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Left(SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Updated(SocketGuild oldguild, SocketGuild newguild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Available(SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Unavailable(SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}