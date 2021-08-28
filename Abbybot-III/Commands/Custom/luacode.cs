using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;

using MoonSharp.Interpreter;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom
{
	[Capi.Cmd("luacode", 1, 1)]
	class luacode : BaseCommand
	{
		int azr = 0;

		public override async Task DoWork(AbbybotCommandArgs message)
		{
			azr = 0;
			UserData.RegisterType<AbbybotUser>();
			UserData.RegisterType<AbbybotGuild>();
			Script script = new(CoreModules.Preset_SoftSandbox); //no io/sys calls allowed
			script.Options.DebugPrint = async s =>
			{
				if (azr < 3)
					await message.Send(s); //when print is used send message
				azr++;
				await Task.Delay(100);
			};
			Abbybot.print($"user {message.user}, guild {message.guild}");
			DynValue user = UserData.Create(message.user);
			script.Globals.Set("user", user);

			if (message.guild != null)
			{
				DynValue guild = UserData.Create(message.guild);
				script.Globals.Set("guild", guild);
			}

			script.Globals["buildfc"] = (Func<string, string>)message.BuildAbbybooruTag;

			DynValue dada = script.DoString("say = print");

			var m = message.Message.Split("```");
			foreach (var item in m)
			{
				var lu = item.Split('\n');
				if (lu[0] != "lua") continue;

				StringBuilder sb = new StringBuilder(item);
				sb.Remove(0, 4); 

				try
				{
					DynValue d = script.DoString(sb.ToString());
				}
				catch (ScriptRuntimeException e)
				{
					Console.WriteLine(e);
					await message.Send("I had a hard time reading your lua master im sorry...");
				}
			}
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs cea)
		{
			Multithreaded = true;
			var v = cea.Message.Contains("```lua");

			if (v)
				return await base.Evaluate(cea);
			else
				return false;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return "I can run your lua blocks of code";
		}
	}
}