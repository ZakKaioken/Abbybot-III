using Abbybot_III.Core.Twitter.Queue.types;

using AbbySql;
using AbbySql.Types;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue.sql
{
    class TweetQueueSql
    {
        public static async Task Add(string message, Image I)
        {
            var url = AbbysqlClient.EscapeString(I.url);
            var sourceurl = AbbysqlClient.EscapeString(I.sourceurl);
            var msg = AbbysqlClient.EscapeString(message);
            await AbbysqlClient.RunSQL($"INSERT INTO `twitter`.`tweets` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority`, `md5`, `GelId` ) VALUES('{url}', '{sourceurl}', '{msg}','0', '{I.md5}', '{I.gelId}');");
        }

        public static async Task<int> Count()
        {
            var table = await AbbysqlClient.FetchSQL("SELECT COUNT(*) as 'rows' FROM `twitter`.`tweets`;");
            int rows = 0;
            foreach (AbbyRow row in table)
                rows = int.Parse(row["rows"].ToString());
            return rows;
        }

        public static async Task Remove(Tweet I)
        {
            await AbbysqlClient.RunSQL($"DELETE FROM `twitter`.`{I.source}` WHERE `Id` = '{I.id}';");
        }

        public static async Task<Tweet> Peek()
        {
            Tweet tweet = null;
            var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `twitter`.`tweets` ORDER BY Priority DESC, Id ASC LIMIT 1;");
            if (table.Count < 1)
                throw new Exception("no tweets in list");

            foreach (AbbyRow row in table)
            {
                tweet = new Tweet()
                {
                    id = (int)row["Id"],
                    url = (row["ImgUrl"] is string i) ? i : "",
                    sourceurl = (row["SrcUrl"] is string s) ? s : "",
                    message = (row["Description"] is string m) ? m : "",
                    priority = (sbyte)row["Priority"] == 1 ? true : false,
                    GelId = (row["GelId"] is int gild ? gild : 0),
                    md5 = (row["md5"] is string smd5) ? smd5 : "",
                    source = "tweets"
                };
            }
            return tweet;
        }

        public static async Task Add(Tweet I, bool v)
        {
            int priority = v ? 1 : 0;
            var url = AbbysqlClient.EscapeString(I.url);
            var sourceurl = AbbysqlClient.EscapeString(I.sourceurl);
            var message = AbbysqlClient.EscapeString(I.message);
            await AbbysqlClient.RunSQL($"INSERT INTO `twitter`.`tweets` ( `ImgUrl`,`SrcUrl`, `Description`, `Priority`, `GelId`, `md5` ) VALUES('{url}', '{sourceurl}', '{message}', '{priority}','{I.GelId}','{I.md5}' );");
        }
    }
}