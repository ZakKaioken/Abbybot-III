using Abbybot_III.Clocks.Guild.User;

using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
	class ClockIniter
	{
		static BaseClock[] clocks = new BaseClock[]
		{
            //new TwitterMentionClock()
            //new MostActiveUserClock(),
			new PingAbbybotClock()
		};

		public static async Task init()
		{
			foreach (var clock in clocks)
			{
				await clock.Init();
			}
		}
	}
}