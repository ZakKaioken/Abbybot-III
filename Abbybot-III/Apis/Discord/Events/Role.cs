using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Role
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.RoleCreated += async (role) => await Role.Created(role);
            _client.RoleDeleted += async (role) => await Role.Deleted(role);
            _client.RoleUpdated += async (oldrole, newrole) => await Role.Updated(oldrole, newrole);
        }

        static async Task Created(SocketRole role)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Deleted(SocketRole role)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Updated(SocketRole oldrole, SocketRole newrole)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}