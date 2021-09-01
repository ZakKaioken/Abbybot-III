namespace Abbybot_III.Core.Twitter.Queue.types
{
    class Tweet
    {
        public int id;
        public int GelId;
        public string url;
        public bool priority;
        public string sourceurl;
        public string message;
        public string md5;
		internal string source;
	}
}