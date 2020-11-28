using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;

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
    [Capi.Cmd("%fcm", 1, 1)]
    class fcm : ContainCommand
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
                "not", "negative", "-", "opposite", "undo"
        };
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message).Replace(Command, "");

            if (FavoriteCharacter.Length < 1)
            {
                var e = await FCMentionsSql.GetFCMAsync(a.abbybotUser.Id);
                var word = (e) ? activationwords.random() : deactivatewords.random();
                await a.Send($"your favorite character mentions modifier is {word}");
                return;
            }
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[^1] == ' ')
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

            string fc = FavoriteCharacter.ToString().ToLower();

            if (fc.Contains("words"))
            {
                FavoriteCharacter.Clear();
                bool act = fc.Contains(activationwords);
                bool disact = fc.Contains(deactivatewords);
                bool negative = fc.Contains(negativewords);

                bool all = act || disact || negative;

                AppendDefinition(FavoriteCharacter, activationwords, act, all);
                AppendDefinition(FavoriteCharacter, deactivatewords, disact, all);
                AppendDefinition(FavoriteCharacter, negativewords, negative, all);
                
                await a.Send(FavoriteCharacter);
                return;
            }


            bool wordused = false; bool state = false;
            foreach (var ac in activationwords){
                if (fc.Contains(ac))
                {
                    wordused = true;
                    state = true;
                }
            }
            foreach (var af in deactivatewords)
            {
                if (fc.Contains(af))
                {
                    wordused = true;
                    state = false;
                }
            }
            if (!wordused)
            {
                await a.Send("You're confusing me master... :(\ndid you forget an activation or disactivation word? (**on**, **off**, **true**, **false**, **enable**, **disable**)");
                return;
            }
            foreach (var ad in negativewords)
            {
                if (fc.Contains(ad))
                {
                    state = !state;
                }
            }

            await FCMentionsSql.SetFCMAsync(a.abbybotUser.Id, state);
            EmbedBuilder eb = new EmbedBuilder();
            if (state) {
                eb.Title =$"Favorite Character Mentions {activationwords.random()}";
                eb.Color = Color.Green;
                eb.Description = "Hehe master! I will now use whoever you mention's fc when you use a picture command!";
            } else {
                eb.Title = $"Favorite Character Mentions {deactivatewords.random()}";
                eb.Color = Color.Red;
                eb.Description = "oh master! I will not use whoever you mention's fc when you use a picture command anymore...";
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
            return $"{Command}: lets you set your favorite character mentions modifier. Usage (%fcm on) to use your mentioned user for pic commands";
        }
    }
}
