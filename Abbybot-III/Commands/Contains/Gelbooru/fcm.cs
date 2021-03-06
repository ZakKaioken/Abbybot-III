﻿using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;

using Discord;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
    [Capi.Cmd("abbybot fcm", 1, 1)]
    class fcm : ContainCommand
    {
        string[] activationwords = new string[]
        {
                "on", "enabled", "enable", "true", "activate",
                "activated", "yes", "go", "start", "1"
        };

        string[] deactivatewords = new string[]
        {
                "off", "disabled", "disable", "false", "stop", "end",
                "finish", "0"
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
                var word = (await a.GetFCMentions()) ? activationwords.random() : deactivatewords.random();
                await a.Send($"your favorite character mentions modifier is {word}");
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
                state = await a.GetFCMentions();
            }
            foreach (var ad in negativewords)
            {
                if (fc.Contains(ad))
                {
                    state = !state;
                }
            }

            await a.SetFCMentions(state);
            EmbedBuilder eb = new EmbedBuilder();
            if (state)
            {
                eb.Title = $"Favorite Character Mentions {activationwords.random()}";
                eb.Color = Color.Green;
                eb.Description = "Hehe master! I will now use whoever you mention's fc when you use a picture command!";
            }
            else
            {
                eb.Title = $"Favorite Character Mentions {deactivatewords.random()}";
                eb.Color = Color.Red;
                eb.Description = "oh master! I will not use whoever you mention's fc when you use a picture command anymore...";
            }

            await a.Send(eb);
        }

        void AppendDefinition(StringBuilder FavoriteCharacter, string[] activationwords, bool act, bool all)
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
            return $"Turn on or off the ability to use the mentioned person's fc instead for pictures commands";
        }
    }
}