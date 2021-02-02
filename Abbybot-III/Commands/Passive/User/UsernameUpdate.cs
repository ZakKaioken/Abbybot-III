﻿using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    [Cmd("UsernameUpdate", 1, 1)]
    class UsernameUpdate : PassiveCommand
    {
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            if (aca.abbybotGuild != null)
                if (!aca.abbybotGuild.AbbybotIsHere)
                    await PassiveUserSql.SetUsernameSql(aca.abbybotUser.Id, aca.abbybotUser.userNames.Username, aca.abbybotUser.userNames.Nickname);
        }
    }
}
