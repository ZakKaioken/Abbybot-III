using Abbybot_III.Core.Data.User;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class UserTrustSql
    {
        public static async Task<AbbybotUser> GetUserTimeout(AbbybotUser au)
        {
            var abisb = new StringBuilder();
            abisb.Append($"SELECT * FROM `usertimeout` WHERE `UserId` = '{au.Id}';");
            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            
            if (table.Count > 0)
            {
                au.userTrust = new Data.User.Subsets.UserTrust();
                au.userTrust.inTimeOut = true;
                au.userTrust.TimeOutEndDate = (table[0]["Time"] is string s) ? DateTime.Parse(s) : DateTime.Now;
                au.userTrust.timeoutReason = (table[0]["Reason"] is string z) ? z : "";
            }

            if (au.userTrust.TimeOutEndDate < DateTime.Now)
            {
                await AbbysqlClient.RunSQL($"DELETE FROM `usertimeout` WHERE `UserId` = '{au.Id}'");
                au.userTrust.inTimeOut = false;
            }

            return au;
        }


    }
}
