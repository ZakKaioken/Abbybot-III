using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.GuildMessageHandler;
using Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User;
using Abbybot_III.Sql.Abbybot.Guild;

using Discord.Rest;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class User
    {
        
        private static async Task Joined(SocketGuildUser user)
        {
            var g = user.Guild;
            var isx = (await g.GetInvitesAsync()).ToList();
            string code = "nocode";
            foreach (var i in isx)
            {
                var laststate = await InviteCounterSql.GetGuild(g, i);
                if (laststate < i.Uses)
                {
                    code = i.Code;
                    if (laststate < 1)
                        await InviteCounterSql.AddGuild(g, i);
                    else 
                        await InviteCounterSql.UpdateGuildName(g, i);
                }
            }
            //this is where you use the code
            JoinedMessage jm = await JoinedMessage.CreateFromUser(user, code);
            
            await MessageHandler.DoGuildMessage(jm);
        }

        private static async Task Left(SocketGuildUser user)
        {
            LeftMessage lm = await LeftMessage.CreateFromUser(user);
            await MessageHandler.DoGuildMessage(lm);
        }

        private static async Task Banned(SocketUser user, SocketGuild guild)
        {
            BannedMessage bm = await BannedMessage.CreateFromUser(user, guild);
        }

        private static async Task Unbanned(SocketUser user, SocketGuild guild)
        {
            UnbannedMessage bm = await UnbannedMessage.CreateFromUser(user, guild);
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