using Abbybot_III.Core.Data.User;

using Capi.Interfaces;

using System.Collections.Generic;

class PictureCommandData
{
	public string message;
	public List<CommandRatings> ratings;
	public string favoriteCharacter;

	public string[] badTags = new string[] {
		"sofra", "large_breasts"
	};

	public List<AbbybotUser> mentions = new List<AbbybotUser>();
	public string channelFavoriteCharacter;
	public bool SelfRun = true;
	public bool Multithreaded = true;
	public bool Rolling = true;
	public ulong index;
	public bool isNSFW;
	public bool isLoli;
	public bool isGuildChannel = false;

	public AbbybotUser user;
}