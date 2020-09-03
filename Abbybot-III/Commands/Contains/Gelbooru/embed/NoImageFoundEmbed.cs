using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru.embed
{
    class NoImageFoundEmbed
    {
        internal static async Task Build(AbbybotCommandArgs aca, string fc)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Title = "No image found... :(";
            eb.Description = $"I could not find a picture of {fc}";
            eb.Color = Color.Blue;
            await aca.Send(eb);
        }
    }
}
