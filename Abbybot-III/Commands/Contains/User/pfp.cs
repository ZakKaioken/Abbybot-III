﻿
using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("abbybot avatar", 1, 1)]
    class avatar : ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            var mu = a.mentionedUserIds.ToArray();
            if (mu.Length < 1)
                return;
            await a.Send(mu[0].GetAvatarUrl());
        }
    }
}