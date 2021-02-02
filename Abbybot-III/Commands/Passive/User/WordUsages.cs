using Abbybot_III.Commands.Contains;
using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Passive.User
{
    [Cmd("worduses", 1,1)]
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
            if(aca.abbybotGuild != null)
                if (!aca.abbybotGuild.AbbybotIsHere)
            foreach (var w in words)
                if (aca.Message.ToLower().Contains(w.word))
                    await PassiveUserSql.IncStat(aca.abbybotUser.Id, w.column);
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
