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
            await AbbysqlClient.RunSQL($"INSERT INTO `tweets` ( `ImgUrl`,`SrcUrl`, `Description` ) VALUES('{I.url}', '{I.sourceurl}', '{message}';");
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
            await AbbysqlClient.RunSQL($"DELETE FROM `tweets` WHERE `Id` = '{I.id}';");
        }

        public static async Task<Tweet> Peek()
        {
            Tweet tweet = null;
            var table = await AbbysqlClient.FetchSQL($"SELECT Id, ImgUrl, SrcUrl, Description FROM `tweets`ORDER BY Id LIMIT 1");
            foreach (AbbyRow row in table)
            {
                tweet = new Tweet()
                {
                    id = (int)row["Id"],
                    url = (row["ImgUrl"] is string i) ? i : "",
                    sourceurl = (row["ImgUrl"] is string s) ? s : "",
                    message = (row["ImgUrl"] is string m) ? m : ""
                };
            }
            return tweet;
        }
    }
}
