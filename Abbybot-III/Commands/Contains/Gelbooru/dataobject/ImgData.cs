using Abbybot_III.Core.Data.User;

using System.Collections.Generic;

namespace Abbybot_III.Commands.Contains.Gelbooru.dataobject
{
    public class ImgData
    {
        public string command;
        public string Imageurl;
        public string source;
        public bool safe;
        public bool nsfw;
        public bool loli;
        public bool shot;
        public AbbybotUser user;
        public string favoritecharacter;
        public AbbybotUser sudouser;
        public List<AbbybotUser> mentions;
    }
}