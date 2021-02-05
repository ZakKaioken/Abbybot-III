using Abbybot_III.Apis.Booru;
using Abbybot_III.Clocks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;

using AbbySql;

using Abyplay;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
    [Capi.Cmd("abbybot autofcdm", 1, 1)]
    class autofcdm : ContainCommand
    {
        string[] activationwords = new string[]
        {
                "on", "enabled", "enable", "true", "activate",
                "activated", "yes", "go", "start"
        };
        string[] deactivatewords = new string[]
        {
                "off", "disabled", "disable", "false", "stop", "end",
                "finish"
        };
        string[] negativewords = new string[]
        {
                "not", "negative", "-", "opposite", "undo", "flip", 
                "nevermind", "reverse"

        };
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message).Replace(Command, "");

            if (FavoriteCharacter.Length < 1)
            {
                var e = await AutoFcDmSqls.GetAutoFcDmAsync(a.abbybotUser.Id);
                var word = (e) ? activationwords.random() : deactivatewords.random();
                await a.Send($"your auto favorite character dm is {word}");
                return;
            }
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[^1] == ' ')
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

            string fc = FavoriteCharacter.ToString().ToLower();

            bool act = fc.Contains(activationwords);
            bool disact = fc.Contains(deactivatewords);
            bool negative = fc.Contains(negativewords);

            bool all = act || disact || negative;
            if (fc.Contains("words"))
            {
                FavoriteCharacter.Clear();


                AppendDefinition(FavoriteCharacter, activationwords, act, all);
                AppendDefinition(FavoriteCharacter, deactivatewords, disact, all);
                AppendDefinition(FavoriteCharacter, negativewords, negative, all);
                
                await a.Send(FavoriteCharacter);
                return;
            }


            bool wordused = false; bool state = false;
                if (act)
                {
                    wordused = true;
                    state = true;
                }
            
                if (disact)
                {
                    wordused = true;
                    state = false;
                }
            
            if (!wordused)
            {
                state = await FCMentionsSql.GetFCMAsync(a.abbybotUser.Id);
            }
            foreach (var ad in negativewords)
            {
                if (fc.Contains(ad))
                {
                    state = !state;
                }
            }

            await AutoFcDmSqls.SetAutoFcDmAsync(a.abbybotUser.Id, state);
            EmbedBuilder eb = new EmbedBuilder();
            if (state) {
                eb.Title =$"Auto Favorite character Dms {activationwords.random()}";
                eb.Color = Color.Green;
                eb.Description = $"Hehe master! I will now send you pictures of {a.abbybotUser.userFavoriteCharacter.FavoriteCharacter} every 15 minutes!!";
            } else {
                eb.Title = $"Auto Favorite character Dms  {deactivatewords.random()}";
                eb.Color = Color.Red;
                eb.Description = $"oh master! I understand... I will not send you pictures of {a.abbybotUser.userFavoriteCharacter.FavoriteCharacter} every 15 minutes anymore...";
            }
        
            await a.Send(eb);
        }

        private void AppendDefinition(StringBuilder FavoriteCharacter, string[] activationwords, bool act, bool all)
        {
            if (act || !all)
            {
                FavoriteCharacter.Append($"**{activationwords.random()}** words: \n");
                foreach (var acti in activationwords)
                {
                    FavoriteCharacter.Append("**").Append(acti).Append("** ");
                }
                FavoriteCharacter.Append("\n");
            }
        }

        
        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {

            var autofcdm = ClockIniter.clocks.Where(x => x.name == "autofcdm").ToList();
            if (autofcdm.Count() < 1) return "get random pics of your waifu in the dms at a certain time";
            var dt = TimeStringGenerator.MilistoTimeString((decimal)(autofcdm[0] as AutoFcDmClock).delay.TotalMilliseconds);

            return $"Get random pictures of your waifu every {dt}";
        }
    }
}
