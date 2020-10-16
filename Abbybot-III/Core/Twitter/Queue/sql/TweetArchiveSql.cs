using Abbybot_III.Core.Twitter.Queue.types;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue.sql
{
    class TweetArchiveSql
    {
        public static async Task Add(string message, Image I)
        {
            var url = AbbysqlClient.EscapeString(I.url);
            var sourceurl = AbbysqlClient.EscapeString(I.sourceurl);
            var msg = AbbysqlClient.EscapeString(message);
            await AbbysqlClient.RunSQL($"INSERT INTO `abbybottwitter`.`tweets` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority` ) VALUES('{url}', '{sourceurl}', '{msg}','0');");
        }

        public static async Task<int> Count()
        {
            var table = await AbbysqlClient.FetchSQL("SELECT COUNT(*) as 'rows' FROM AbbybotTwitter.tweets;");
            int rows = 0;
            foreach (AbbyRow row in table)
                rows = int.Parse(row["rows"].ToString());
            return rows;
        }

        public static async Task Remove(Tweet I)
        {
            await AbbysqlClient.RunSQL($"DELETE FROM `abbybottwitter`.`tweets` WHERE `Id` = '{I.id}';");
        }
        static Random r = new Random();

        public static async Task<Tweet> Peek()
        {
            Tweet tweet = null;
            var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybottwitter`.`tweetarchive`");
            if (table.Count < 1)
                throw new Exception("no tweets in list");
            
            AbbyRow row = table[r.Next(0, table.Count)];
            tweet = new Tweet()
                {
                    id = (int)row["Id"],
                    url = (row["ImgUrl"] is string i) ? i : "",
                    sourceurl = (row["SrcUrl"] is string s) ? s : "",
                    message = (row["Description"] is string m) ? m : "",
                    priority = (sbyte)row["Priority"] == 1 ? true : false
                };
            
            return tweet;
        }

        internal static async Task Add(Tweet I, bool v)
        {

            int priority = v ? 1 : 0;
            var url = AbbysqlClient.EscapeString(I.url);
            var sourceurl = AbbysqlClient.EscapeString(I.sourceurl);
            var message = AbbysqlClient.EscapeString(I.message);

            var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybottwitter`.`tweetarchive` WHERE `ImgUrl` = '{url} AND `Description` = '{message}';");
            if (table.Count > 0)
                return;

            await AbbysqlClient.RunSQL($"INSERT INTO `abbybottwitter`.`tweetarchive` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority` ) VALUES('{url}', '{sourceurl}', '{message}', '{priority}');");
        }
    }
}
