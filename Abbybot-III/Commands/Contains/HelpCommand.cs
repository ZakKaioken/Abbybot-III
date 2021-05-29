using Abbybot_III.Core.CommandHandler;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

using Discord;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
	[Capi.Cmd("abbybot help", 1, 1)]
	class HelpCommand : ContainCommand
	{
		public HelpCommand()
		{
			Multithreaded = true;
		}

		public override bool SelfRun { get => false; set => base.SelfRun = value; }

		public override async Task DoWork(AbbybotCommandArgs abd)
		{
			EmbedBuilder eb = new EmbedBuilder
			{
				Title = "Almost every command i will listen to",
				Color = Color.Teal
			};
			var au = abd.user;
			if (abd.user.inTimeOut)
			{
				eb.Color = Color.Red;
				eb.Title = "Your timeout commands.";
				eb.Description = "You've been mean to me...";
				eb.AddField("\u200b", "abbybot help\nabbybot timeout");
				await abd.SendDM(eb);
				return;
			}
			if (CommandHandler.capi.commands.Count == 1)
			{
				eb.Color = Color.Red;
				eb.Title =
				eb.Description = "Aawawawa!!! master!! I lost all my commands... Help me... I'M PANICING!!!!";
				await abd.Send(eb);
				return;
			}
			await abd.Send("Hey cutie master! I'll give you my commands when i find them all!! 😁");

			var ratings = abd.user.Ratings;
			List<iCommand> commands = CommandHandler.capi.commands.ToList();

			StringBuilder currentitem = new StringBuilder();
			bool groupnameadded = false;
			List<StringBuilder> fields = new List<StringBuilder>();

			foreach (var command in commands)
			{
				await command.RegenHelpString(abd);
			}

			var groups = commands.OrderBy(x => x.Type).GroupBy(x => x.helpString).ToList(); ;
			await Task.Delay(100);
			fields.Add(new StringBuilder());

			foreach (var group in groups.ToList())
			{
				var groop = group.ToList().OrderBy(xx => xx.Command.Replace("abbybot ", "")).ToList();
				if (group.Count() > 1)
				{
					groupnameadded = false;
					for (int ooo = 0; ooo < groop.Count; ooo++)
					{
						var item = groop[ooo];
						if (!await item.ShowHelp(abd))
							continue;
						if (!groupnameadded)
						{
							var helpstring = (groop[0].helpString).Replace("abbybot ", "ab!");
							fields[^1].AppendLine(helpstring);
							groupnameadded = true;
						}

						currentitem.Clear();
						var cos = item.Command.Replace("abbybot ", "ab!");
						currentitem.Append($"**{cos}**");
						if (ooo < groop.Count - 1)
							currentitem.Append(", ");
						else currentitem.Append("\n");

						NewMethod();
						if (!fields[^1].ToString().Contains(currentitem.ToString()))
							fields[^1].Append(currentitem);
					}
				}
				else
				{
					if (!await groop[0].ShowHelp(abd)) continue;

					currentitem.Clear();
					var command = groop[0].Command.Replace("abbybot ", "ab!");
					var helpstring = (groop[0].helpString).Replace("abbybot ", "ab!");
					currentitem.Append("\n").Append(command).Append(": ").Append(helpstring);
					currentitem.Replace(command, $"**{command}**");
					NewMethod();

					if (!fields[^1].ToString().Contains(currentitem.ToString()))
						fields[^1].Append(currentitem);
					currentitem.Clear();
				}
				if (currentitem.Length > 1)
				{
					NewMethod();
					if (fields.Count > 1)
						if (!fields[^2].ToString().Contains(currentitem.ToString()))
							fields[^1].Append(currentitem).Append("\n");
					currentitem.Clear();
				}
			}

			foreach (var fff in fields.Where(fzf => fzf.Length <1)) {
				fields.Remove(fff);
			}

			foreach (var f in fields)
			{
				eb.AddField("\u200b", f.Replace("ab!", "%"));
			}
			await abd.SendDM(eb);

			if (abd.user.inTimeOut)
				await abd.Send("I put it in our dms.");

			void NewMethod()
			{
				var e = (currentitem.Length + fields[^1].Length);
				if (e > 1000)
				{
					fields.Add(new StringBuilder());
					groupnameadded = false;
				}
			}
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
		{
			return aca.Message.ToLower().Contains(Command.ToLower());
		}

		public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
		{
			return true;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"{Command} displays a list of possible commands to your dms. if you're seeing this, you likely ran the help command!";
		}
	}

	static class LinqExtensions
	{
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
		{
			int i = 0;
			var splits = from item in list
						 group item by i++ % parts into part
						 select part.AsEnumerable();
			return splits;
		}
	}
}