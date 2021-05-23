using System;
using System.Linq;

class ImageData
{
	public string pictureCharacter;
	public Uri FileUrl;
	public Uri PreviewUrl;
	public string Source;
	public string[] Tags;
	public bool Nsfw;

	public bool ContainsLoli
	{
		get
		{
			return Tags.Contains("loli") && Tags.Contains("shota");
		}
	}
}