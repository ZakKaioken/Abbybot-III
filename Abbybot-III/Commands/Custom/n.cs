using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom
{
	[Capi.Cmd("n", 1, 1)]
	class n : ContainCommand
	{
		StringBuilder nhen = new StringBuilder();
		List<(string title, string ptitle, Uri cover, string[] tags, int id)> books = new();

		public override async Task DoWork(AbbybotCommandArgs message)
		{
			nhen.Clear();
			foreach ((string title, string ptitle, Uri cover, string[] tags, int id) in books)
			{
				EmbedBuilder eb = new();
				eb.Title = ptitle;
				eb.ImageUrl = cover.ToString();
				eb.Color = new Color(237, 37, 83);

				StringBuilder sb = new(string.Join(", ", tags));
				int incs = 34;
				int boincs = (int)Math.Floor(sb.Length / (float)incs);
				for (int i = 1; i < boincs; i++)
				{
					sb.Insert(incs * i, "\n");
				}
				eb.Footer = new EmbedFooterBuilder() {
					Text = sb.ToString(),
					IconUrl = "https://pbs.twimg.com/profile_images/733172726731415552/8P68F-_I_400x400.jpg"
				};
				
				nhen.AppendLine($"https://nhentai.net/g/{id}");
				await message.Send(nhen, eb);
			}
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
				//Console.WriteLine($"{cmd}: id: {book[1]}");
				if (int.TryParse(book[1], out int o))
				{
					//Console.WriteLine("converted to int");
					var b = await IsHentai(o, 
					e=>Console.WriteLine($"<<<<<{e}>>>>>"));
					if (b.title != null)
						books.Add((b.title, b.ptitle, b.cover, b.tags, o));
				}else {
					//Console.WriteLine("failed to converted to int");
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

		public async Task<(string title, string ptitle, Uri cover, string[] tags)> IsHentai(int book, Action<Exception> onFail=null)
		{
			StringBuilder sb = new();
			try
			{
			var boog = await NHentaiSharp.Core.SearchClient.SearchByIdAsync(book);

				return (boog.englishTitle, boog.prettyTitle, boog.cover.imageUrl, boog.tags.Select(t => t.name).ToArray());
			}
			catch (Exception e) { onFail?.Invoke(e); }
			return (null, null, null, null);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return "just add an n in front of an nhentai number and i will give you it's link. It doesn't work with numbers that aren't hentais on nhentai";
		}
	}
}