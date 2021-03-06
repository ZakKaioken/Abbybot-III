﻿using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Passive.User
{
    [Cmd("worduses", 1, 1)]
    class WordUsages : PassiveCommand
    {
        Word[] words = new Word[]
        {
            new Word("loli", "LoliWordUsages"),
            new Word("abby", "AbbyNameUsages"),
            new Word("abigail", "AbbyNameUsages")
        };

        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            ulong guildId = 0, channelId = 0;

            if (aca.guild != null)
            {
                guildId = aca.guild.Id;
                channelId = aca.channel.Id;
            }
            foreach (var w in words)
                if (aca.Message.ReplaceA("abbybot ", "").ToLower().Contains(w.word))
                    await aca.IncreasePassiveStat(w.column);
        }
    }

    class Word
    {
        public string word;
        public string column;

        public Word(string word, string column)
        {
            this.word = word;
            this.column = column;
        }
    }
}