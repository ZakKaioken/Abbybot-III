using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.GuildMessageHandler;
using Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User;

using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class User
    {

        private static async Task Joined(SocketGuildUser user)
        {
            JoinedMessage jm = await JoinedMessage.CreateFromUser(user);
            await MessageHandler.DoGuildMessage(jm);
        }

        private static async Task Left(SocketGuildUser user)
        {
            LeftMessage lm = await LeftMessage.CreateFromUser(user);
            await MessageHandler.DoGuildMessage(lm);
        }

        private static async Task Banned(SocketUser user, SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Unbanned(SocketUser user, SocketGuild guild)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Updated(SocketUser olduser, SocketUser newuser)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }


        internal static void Init(DiscordSocketClient _client)
        {

            _client.UserJoined += async (user) => await Joined(user);
            _client.UserLeft += async (user) => await Left(user);
            _client.UserBanned += async (user, guild) => await Banned(user, guild);
            _client.UserUnbanned += async (user, guild) => await Unbanned(user, guild);
            _client.UserUpdated += async (olduser, newuser) => await Updated(olduser, newuser);
        }

    }
}