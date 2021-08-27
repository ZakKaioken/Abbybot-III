﻿using Abbybot_III.Apis.Events;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.RequestSystem;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.Abbybot;
using Abbybot_III.Sql.Abbybot.User;

using BooruSharp.Search.Post;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler.extentions
{
    public static class AbbybotCommandArgsExtentions
    {
        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, object st)
        {
            //Abbybot.print(st);
            if (st.ToString().Length < 1)
                return null;
            return await arg.channel.SendMessageAsync(st.ToString());
        }
public static async Task<List<RestUserMessage>> Send<t>(this AbbybotCommandArgs arg, List<t> st)
        {
            if (st==null) return null;
            List<RestUserMessage> rums = new();
            foreach (var item in st)
            {
                var ww = await Send(arg: arg, item);
                if (item!=null&&ww!=null)
                rums.Add(ww);
            }
            return rums;
        }

        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, EmbedBuilder eb)
        {
            if (eb == null)
                return null;
            return await arg.channel.SendMessageAsync(null, false, eb.Build());
        }

        public static async Task<RestUserMessage> Send(this AbbybotCommandArgs arg, StringBuilder sb)
        {
            //Abbybot.print(sb);
            if (sb.Length < 1)
                return null;
            return await arg.channel.SendMessageAsync(sb.ToString());
        }

        public static async Task Send(this AbbybotCommandArgs arg, RequestObject ro)
        {
            await Task.FromResult(ro);
            RequestCore.AddRequest(ro);
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, object st)
        {
            if (st.ToString().Length < 1)
                return null;
            return await arg.author.SendMessageAsync(st.ToString());
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, EmbedBuilder eb)
        {
            return await arg.author.SendMessageAsync(null, false, eb.Build());
        }

        public static async Task<IUserMessage> SendDM(this AbbybotCommandArgs arg, StringBuilder sb)
        {
            return (sb.Length < 1) ? null : await arg.author.SendMessageAsync(sb.ToString());
        }
        
		public static string[] Split(this AbbybotCommandArgs arg, string split, bool lowercase=false) {
			return (lowercase) ? arg.Message.ToLower().Split(split.ToLower()) : arg.Message.Split(split);
		}

		public static bool Contains(this AbbybotCommandArgs arg, string item, bool lowercase = false)
		{
			return (lowercase) ? arg.Message.ToLower().Contains(item.ToLower(), System.StringComparison.InvariantCultureIgnoreCase) : arg.Message.Contains(item);
		}
        public static bool Contains(this AbbybotCommandArgs arg, string[] items, bool lowercase = false)
        {
            return items.Any(x => arg.Contains(x, lowercase));
        }

        public static StringBuilder Replace(this AbbybotCommandArgs arg, string item, string replacement, bool lowercase = false)
        {
            StringBuilder sb;
            if (lowercase) sb = new StringBuilder(arg.Message.ToLower()).Replace(arg.Message.ToLower(), "");
            else sb = new StringBuilder(arg.Message).Replace(arg.Message, "");
            if (sb.Length > 0)
            {
                while (sb[0] == ' ')
                    sb.Remove(0, 1);
                while (sb.ToString().Contains("  "))
                    sb.Replace("  ", " ");
                while (sb[^1] == ' ')
                    sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }
        public static StringBuilder Replace(this AbbybotCommandArgs arg, string item, bool lowercase = false)
        {
            StringBuilder sb;
            if (lowercase) sb = new StringBuilder(arg.Message.ToLower()).Replace(item.ToLower(), "");
            else sb = new StringBuilder(arg.Message).Replace(item, "");

            if (sb.Length > 0)
            {
                while (sb[0] == ' ')
                    sb.Remove(0, 1);
                while (sb.ToString().Contains("  "))
                    sb.Replace("  ", " ");
                while (sb[^1] == ' ')
                    sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }
        public static string[] GetFCList(this AbbybotCommandArgs arg)
        {
            return arg.user.FavoriteCharacter.Replace("{", $@"").Replace("}", "").Split(" ~ ");
        }

        public static string BuildAbbybooruTag(this AbbybotCommandArgs arg, string english)
        {
            return AbbybooruTagGenerator.FCBuilder(english);
        }
        public static StringBuilder BuildAbbybooruTag(this AbbybotCommandArgs arg, StringBuilder english)
        {
            return AbbybooruTagGenerator.FCBuilder(english);
        }

        public static string BreakAbbybooruTag(this AbbybotCommandArgs arg, string s)
        {
            return arg.BreakAbbybooruTag(new StringBuilder(s)).ToString();
        }
        public static StringBuilder BreakAbbybooruTag(this AbbybotCommandArgs arg, StringBuilder s)
        {
            return s.Replace("* ~ ", " or ").Replace("* ", " and ").Replace("{", "").Replace("}", "").Replace("_", " ").Replace("*", "");
        }
        public static string[] ReplaceSplit(this AbbybotCommandArgs arg, string item, string replacement, string split)
        {
            return arg.Replace(item, replacement).ToString().Split(split);
		}
		public static string[] ReplaceSplit(this AbbybotCommandArgs arg, string item, string split)
		{
			return arg.Replace(item, "").ToString().Split(split);
		}


        public static List<SocketGuildUser> getMentionedDiscordGuildUsers(this AbbybotCommandArgs arg)
        {
            List<SocketGuildUser> us = new List<SocketGuildUser>();
            foreach (var item in arg.mentionedUsers)
            {
                us.Add(arg.GetGuildUser(arg.guild.Id, item.Id));
            }
            return us;
        }
        public static async Task<List<AbbybotUser>> getMentionedAbbybotUsers(this AbbybotCommandArgs arg)
        {
            List<AbbybotUser> us = new List<AbbybotUser>();
            foreach (var item in arg.mentionedUsers)
            {
                us.Add(await AbbybotUser.GetUserFromSocketUser(item));
            }
            return us;
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

        public static async Task<List<AbbybotUser>> GetMentionedUsers(this AbbybotCommandArgs aca)
        {
            List<AbbybotUser> mentionedUsers = new List<AbbybotUser>();

            foreach (var u in aca.mentionedUsers)
            {
                AbbybotUser au = null;
                if (u is SocketGuildUser sgu)
                {
                    au = await AbbybotUser.GetUserFromSocketGuildUser(sgu);
                }
                else
                {
                    au = await AbbybotUser.GetUserFromSocketUser(u);
                }
                mentionedUsers.Add(au);
            }

            return mentionedUsers;
        }

        public static SocketGuild GetGuild(this AbbybotCommandArgs aca, ulong guildId)
        {
            return aca.discordClient.GetGuild(guildId);
        }
        public static async Task<bool> GetFCMentions(this AbbybotCommandArgs aca)
        {
            return await FCMentionosSql.GetFCMAsync(aca.user.Id);
        }
        public static async Task SetFCMentions(this AbbybotCommandArgs aca,bool state)
        {
            await FCMentionosSql.SetFCMAsync(aca.user.Id, state);
        }
        public static async Task<bool> GetAutoFcDms(this AbbybotCommandArgs aca)
        {
            return await AutoFcDmoSqls.GetAutoFcDmAsync(aca.user.Id);
        }

        public static int AbbyRngRoll(this AbbybotCommandArgs aca, int i, int o)
        {
            return ((aca.random == null) ? new Random() : aca.random).Next(i, o);
        }

        public static async Task<List<(ulong channel, ulong stat)>> IncreasePassiveStat(this AbbybotCommandArgs aca, string stat)
        {
            List<(ulong channel, ulong stat)> osi = null;
            try
            {
                await PassiveUserSql.IncreaseStat(aca.abbybotId, aca.guild?.Id != null ? aca.guild.Id : 0, aca.channel.Id, aca.user.Id, stat);
                osi = await PassiveUserSql.GetChannelsinGuildStats(aca.abbybotId, aca.guild?.Id != null ? aca.guild.Id : 0, aca.user.Id, "GelCommandUsages");
            }
            catch { }
            return osi;
        }

        public static async Task<List<(ulong channel, ulong stat)>> GetPassiveStat(this AbbybotCommandArgs aca, string stat)
        {
            List<(ulong channel, ulong stat)> osi = null;
            try
            {
                osi = await PassiveUserSql.GetChannelsinGuildStats(aca.abbybotId, aca.guild?.Id != null ? aca.guild.Id : 0, aca.user.Id, "GelCommandUsages");
            }
            catch { }
            return osi;
        }

        public static async Task<SearchResult> GetPicture(this AbbybotCommandArgs aca, string[] tags)
        {
            Console.WriteLine("get picture ");

            try
            {
                var ts = await Apis.AbbyBooru.ExecuteAsync(tags);

                Console.WriteLine(ts.Count);
                var o = (ts)[0];
                if (ts.Count < 1) throw new Exception("AAAA");
                return o;
            }
            catch (Exception e)
            {
                Console.WriteLine($"no image found... [{e}]");
                throw new Exception("AAAA");
                return new SearchResult(null, null, null, Rating.Safe, null, 0, null, 0, 0, null, null, null, "noimagefound", null, null);
            }

        }
        public static async Task<List<SearchResult>> GetPictures(this AbbybotCommandArgs aca, List<string> tags)
        {
            return await Apis.AbbyBooru.ExecuteAsync(tags.ToArray());
        }
        public static async Task<bool> IsAbbybotHere(this AbbybotCommandArgs aca)
        {
            var abbybotids = await AbbybotSql.GetAbbybotIdAsync();
            abbybotids.Remove(aca.abbybotId);
            bool b = false;
            foreach (var o in abbybotids)
            {
                var u = aca.GetUser(o);
                b = u.MutualGuilds.ToList().Any(x => x.Id == aca.guild.Id);
            }
            return b;
        }
        public static string InvertName(this AbbybotCommandArgs aca, string v)
        {
            var a = AbbybooruTagGenerator.InvertName(v);
            return a;
        }
        public static SocketGuildUser GetGuildUser(this AbbybotCommandArgs aca, ulong guildId, ulong userId)
        {
            return aca.discordClient.GetGuild(guildId).GetUser(userId);
        }
        public static SocketGuildChannel GetGuildChannel(this AbbybotCommandArgs aca, ulong guildId, ulong channelId)
        {
            return aca.discordClient.GetGuild(guildId).GetChannel(channelId);
        }
        public static SocketUser GetUser(this AbbybotCommandArgs aca, ulong userId)
        {
            return aca.discordClient.GetUser(userId);
        }
    }
}