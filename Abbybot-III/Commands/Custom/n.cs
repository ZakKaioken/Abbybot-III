using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom
{
	[Capi.Cmd("n", 1, 1)]
	class n : ContainCommand
	{
		StringBuilder nhen = new StringBuilder();
		List<int> books = new List<int>();

		public override async Task DoWork(AbbybotCommandArgs message)
		{
			nhen.Clear();
			foreach (var b in books)
			{
				nhen.AppendLine($"https://nhentai.net/g/{b}");
			}
			await message.Send(nhen);
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs cea)
		{
			books.Clear();
			var msg = cea.Split(" ", true);
			var cmd = Command.ToLower();
			bool verification = false;
			foreach (var m in msg)
			{
				if (!m.Contains(cmd)) continue;
				var book = m.Split(cmd);
				Console.WriteLine($"{cmd}: id: {book[1]}");
				if (int.TryParse(book[1], out int o))
				{
					Console.WriteLine("converted to int");
					await IsHentai(o, _ =>
					{
						books.Add(o);
					}, e=>Console.WriteLine($"<<<<<{e}>>>>>"));
				}else {
					Console.WriteLine("failed to converted to int");
				}
			}

			bool isnsfwchannel = false;
			if (cea.channel is SocketDMChannel sdc)
				isnsfwchannel = true;
			else if (cea.channel is ITextChannel itc)
				isnsfwchannel = itc.IsNsfw;

			if (books.Count > 0) verification = true;

			if (!isnsfwchannel)
				verification = false;


			return verification;
		}

		public async Task IsHentai(int book, Action<int> onSuccess=null, Action<Exception> onFail=null)
		{
			StringBuilder sb = new();
			try
			{
				var w = await NHentaiSharp.Core.SearchClient.SearchByIdAsync(book);
				onSuccess?.Invoke(book);
			}
			catch (Exception e) { onFail?.Invoke(e); }
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return "just add an n in front of an nhentai number and i will give you it's link. It doesn't work with numbers that aren't hentais on nhentai";
		}
	}
}