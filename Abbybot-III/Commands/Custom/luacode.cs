using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Sql.Abbybot.Fun;

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
			script.Options.DebugPrint = _ => { };
			var luadata = await LuaSql.GetLuaData(message.user.Id);

			foreach (var luaCo in luadata.LuaPieces)
				await TryRunLua(message, script, luaCo);

			script.Options.DebugPrint = async s =>
			{
				if (azr < 3)
				{
					azr++;
					//Console.WriteLine($"test {azr}");
					await message.Send(s); //when print is used send message
				}
				else
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

				StringBuilder sb = new(item);
				sb.Remove(0, 4);

				await TryRunLua(message, script, sb.ToString(), async ww =>
					await LuaSql.AddLuaData(message.user.Id, ww));
			}
		}

		private static async Task TryRunLua(AbbybotCommandArgs message, Script script, string sb, Func<string, Task> OnRan = null)
		{
			try
			{
				DynValue d = script.DoString(sb);
				if (OnRan != null) 
					await OnRan.Invoke(sb);
			}
			catch (ScriptRuntimeException e)
			{
				string reason = CustomizeExceptions(sb, e.DecoratedMessage); 
				await message.Send(reason);
			}
			catch (SyntaxErrorException e) 
			{
				string reason = CustomizeExceptions(sb, e.DecoratedMessage);
				await message.Send(reason);
			}
		}

		private static string CustomizeExceptions(string sb, string ww)
		{
			var zz = ww.Split(":");
			var line = int.Parse(zz[1].Split(",")[0].Remove(0, 1));

			var linePreview = sb.Split("\n")[line - 1];
			var water = zz[2].Split("'");
			var reason =  water[0];

			reason = reason.Remove(0, 1);
			reason = reason switch
			{
				"attempt to call a nil value" => $"master... that function on line {line} doesn't exist...",
				"attempt to call a number value" => $"silly master... that's a number not a function on line {line}!",
				"attempt to perform arithmetic on a string value" => "Silly!! You can't add to strings... \nyou probably meant to insert the value into the string...\nyou can do that using **..** instead. (var1..\" awawa\") if var1 was set to 3 it will output \"3 awawa\".",
				"unfinished string near " => $"I think you forgot to finish the string on line {line}",
				"unexpected symbol near "=> $"I am confused. Something doesnt fit near the '**{water[1]}**' on line {line}",
				_ => $"I had a hard time reading line {line}... im sorry... \n{reason}"
			};
			reason = $"{reason}\n```\n{line}. {linePreview}\n```";
			return reason;
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs cea)
		{
			Multithreaded = true;
			var v = cea.Message.Contains("```lua");

			return v ? await base.Evaluate(cea) : false;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return "I can run your lua blocks of code";
		}
	}
}