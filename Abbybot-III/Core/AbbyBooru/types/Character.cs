using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru.types
{
    class Character
    {
        public ulong Id;
        public bool IsLewd;
        public string tag;
        public ulong guildId;
        public ulong channelId;

        internal static async Task<List<Character>> GetListFromSqlAsync()
        {
            List<Character> characters = new List<Character>();

            AbbyTable table = await AbbysqlClient.FetchSQL("SELECT * FROM AbbyBooruCharacters");
            foreach(AbbyRow row in table)
            {
                var chr = new Character() {
                    Id = (ulong)row["Id"],
                    tag = (row["tag"] is string msg) ? msg : "",
                    channelId = (ulong)row["channelId"],
                    guildId=(ulong)row["guildId"],
                    IsLewd = (sbyte)row["IsLewd"] == 1 ? true : false
                };
                characters.Add(chr);
            }
            return characters;
        }

        internal static async Task<List<ulong>> GetLatestPostIdsAsync(Character character)
        {
            List<ulong> ids = new List<ulong>();

            AbbyTable table = await AbbysqlClient.FetchSQL($"SELECT * FROM CharacterPostIds WHERE CharId = {character.Id};");
            foreach (AbbyRow row in table)
            {
                ids.Add((ulong)row["Id"]);
            }
            return ids;
        }

        internal static async Task AddLatestPostIdAsync(ulong CharId , ulong id)
        {
            await AbbysqlClient.RunSQL($"INSERT INTO CharacterPostIds (Id, CharId) VALUES ('{id}', '{CharId}');");
        }
    }
}
