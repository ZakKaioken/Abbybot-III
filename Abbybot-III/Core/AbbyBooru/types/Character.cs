using AbbySql;
using AbbySql.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru.types
{
    public class Character
    {
        public ulong Id;
        public bool IsLewd;
        public string tag;
        public ulong guildId;
        public ulong channelId;

    }
}