using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Apis.Events
{
    public class Guild
    {
        public static async void Init(DiscordSocketClient _client)
        {
            _client.JoinedGuild += async (guild) => await Guild.Joined(guild);
            _client.LeftGuild += async (guild) => await Guild.Left(guild);
            _client.GuildUpdated += async (oldguild, newguild) => await Guild.Updated(oldguild, newguild);
            _client.GuildAvailable += async (guild) => await Guild.Available(guild);
            _client.GuildUnavailable += async (guild) => await Guild.Unavailable(guild);
            _client.InviteCreated += async (guild) => await Task.CompletedTask;
            _client.InviteDeleted += async (guild, str) => await Task.CompletedTask;
            _client.GuildScheduledEventUserRemove += async (ctx, ctx2) => await Task.CompletedTask;
            _client.GuildScheduledEventUserAdd += async (ctx, ctx2) => await Task.CompletedTask;
            _client.GuildScheduledEventUpdated += async (ctx, ctx2) => await Task.CompletedTask;
            _client.GuildScheduledEventStarted += async (ctx) => await Task.CompletedTask;
            _client.GuildScheduledEventCreated += async (ctx) => await Task.CompletedTask;
            _client.GuildScheduledEventCompleted += async (ctx) => await Task.CompletedTask;
            _client.GuildScheduledEventCancelled += async (ctx) => await Task.CompletedTask;
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