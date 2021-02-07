﻿using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler
{
    class CommandHandler
    {
        public static Capi.Command_Handler capi = new Capi.Command_Handler();
        internal static async Task Handle(SocketMessage message)
        {
            AbbybotCommandArgs aca = await AbbybotCommandArgs.MakeArgsFromMessage(message);
            await UserTrustSql.GetUserTimeout(aca.abbybotUser);
            Console.WriteLine(aca.abbybotUser.userTrust.inTimeOut);
            capi.Start(aca);
        }


    }
}
