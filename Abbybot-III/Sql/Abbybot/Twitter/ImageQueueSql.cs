﻿using Abbybot_III.Core.Twitter.Queue.types;

using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue.sql
{
    class ImageQueueSql
    {
        public static async Task Add(Image I)
        {
            await AbbysqlClient.RunSQL($"INSERT INTO `twitter`.`images` ( `ImgUrl`,`SrcUrl` ) VALUES ('{I.url}','{I.sourceurl}' ); ");
        }

        public static async Task<int> Count()
        {
            var table = await AbbysqlClient.FetchSQL("SELECT COUNT(*) as 'rows' FROM `twitter`.`images`;");
            int rows = 0;
            foreach (AbbyRow row in table)
                rows = int.Parse(row["rows"].ToString());
            return rows;
        }

        public static async Task Remove(Image I)
        {
            await AbbysqlClient.RunSQL($"DELETE FROM `twitter`.`images` WHERE `Id` = '{I.id}';");
        }

        public static async Task<Image> Peek()
        {
            Image image = null;
            AbbyTable table = await AbbysqlClient.FetchSQL($"SELECT * FROM `twitter`.`images`ORDER BY Id LIMIT 1;");
            foreach (AbbyRow row in table)
            {
                image = new Image
                {
                    id = (int)row["Id"],
                    url = (row["ImgUrl"] is string i) ? i : "",
                    sourceurl = (row["ImgUrl"] is string s) ? s : ""
                };
            }
            return image;
        }
    }
}