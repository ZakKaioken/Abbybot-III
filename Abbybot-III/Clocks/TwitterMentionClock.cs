using Abbybot_III.Apis.Twitter;
using Abbybot_III.Core.AbbyBooru.sql;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TweetSharp;

namespace Abbybot_III.Clocks
{
    class TwitterMentionClock : RepeatingClock
    {
        int sendCount = 0;
        int maxSendCount = 5;

        //Queue<>
        public override async Task OnInit(DateTime time)
        {
            delay = TimeSpan.FromMinutes(15);
            await base.OnInit(time);
        }

        public override async Task OnWork(DateTime time)
        {
            var atmms = await Twitter.ts.ListTweetsMentioningMeAsync(new TweetSharp.ListTweetsMentioningMeOptions { Count = 5, IncludeEntities = true });

            TwitterRateLimitStatus rate = Twitter.ts.Response.RateLimitStatus;
            Abbybot.print("You have used " + rate.RemainingHits + " out of your " + rate.HourlyLimit);

            var tsmm = atmms.Value.ToArray();
            foreach (var tmm in tsmm)
            {
                var test = await AbbybotMentionSql.GetLatestMentionIdsAsync((ulong)tmm.Id);
                if (test.Count > 0) continue;
                await AbbybotMentionSql.AddLatestMentionIdAsync((ulong)tmm.Id);

                var tmt = Regex.Replace(tmm.Text, @"http[^\s]+", "");
                var t = Regex.Replace(tmt, @"@[^\s]+", "");
                //var twitteruser = AbbybotUser.GetUserFromTwitterUser(tmm.Author.ScreenName);
                //AbbybotTwitterCommandArgs atca = new AbbybotTwitterCommandArgs();
                //atca.Message = t;
                //atca.abbybotUser = (AbbybotUser)twitteruser;

                Abbybot.print($"[\n{t}\n]");
            }
        }
    }
}