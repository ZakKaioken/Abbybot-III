﻿using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.RequestSystem;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler.extentions
{
    public static class AbbybotCommandArgsExtentions
    {
        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, object st)
        {
            //Console.WriteLine(st);
            return await arg.channel.SendMessageAsync(st.ToString());
        }

        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, EmbedBuilder eb)
        {
            if (eb == null)
                return null;
            return await arg.channel.SendMessageAsync(null, false, eb.Build());
        }

        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, StringBuilder sb)
        {
            //Console.WriteLine(sb);
            return await arg.channel.SendMessageAsync(sb.ToString());
        }
        public static async Task Send(this AbbybotCommandArgs arg, RequestObject ro)
        {
            await Task.FromResult(ro);
            RequestCore.AddRequest(ro);
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, object st)
        {
            return await arg.author.SendMessageAsync(st.ToString());
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, EmbedBuilder eb)
        {
            return await arg.author.SendMessageAsync(null, false, eb.Build());
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, StringBuilder sb)
        {
            return await arg.author.SendMessageAsync(sb.ToString());
        }

        public static async Task Delete(this AbbybotCommandArgs arg)
        {
            await arg.originalMessage.DeleteAsync();
        }


        public static async Task<bool> IsNSFW(this AbbybotCommandArgs arg)
        {

            if (arg.channel is SocketDMChannel sdc)
            {
                return true;
            }
            else if (arg.channel is ITextChannel itc)
            {
                return itc.IsNsfw;
            }
            else
            {
                return false;
            }

        }

    }
}