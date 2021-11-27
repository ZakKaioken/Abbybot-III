using System;
using System.Linq;

public class GelbooruResult {
	public int id; 
	public string md5;
	public Uri FileUrl;
	public Uri PreviewUrl;
	public string Source;
    public string[] tags;
    public bool ContainsLoli
	{
		get
		{
			return tags.Contains("loli") && tags.Contains("shota");
		}
	}
    
	public bool Nsfw;
}