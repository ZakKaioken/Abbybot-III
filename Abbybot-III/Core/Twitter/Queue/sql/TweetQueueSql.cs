using Abbybot_III.Core.Twitter.Queue.types;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue.sql
{
    class TweetQueueSql
    {
        public static async Task Add(string message, Image I)
        {
            await AbbysqlClient.RunSQL($"INSERT INTO `abbybottwitter`.`tweets` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority` ) VALUES('{I.url}', '{I.sourceurl}', '{message}','0');");
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

        public static async Task<Tweet> Peek()
        {
            Tweet tweet = null;
            var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybottwitter`.`tweets` ORDER BY Priority DESC, Id ASC LIMIT 1;");
            foreach (AbbyRow row in table)
            {
                tweet = new Tweet()
                {
                    id = (int)row["Id"],
                    url = (row["ImgUrl"] is string i) ? i : "",
                    sourceurl = (row["ImgUrl"] is string s) ? s : "",
                    message = (row["ImgUrl"] is string m) ? m : "",
                    priority = (sbyte)row["Priority"] == 1 ? true : false
                };
            }
            return tweet;
        }

        internal static async Task Add(Tweet I, bool v)
        {
                int priority = v ? 1 : 0;
                await AbbysqlClient.RunSQL($"INSERT INTO `abbybottwitter`.`tweets` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority` ) VALUES('{I.url}', '{I.sourceurl}', '{I.message}', '{priority}');");
        }
    }
}
