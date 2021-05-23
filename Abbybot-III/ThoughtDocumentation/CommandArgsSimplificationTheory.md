CommandArgs have 1 basic task. Hold the information you need to send to each command for them to work properly. 

The goal is to simplify what we do with our command args. To provide each command with predefined values that are (currently, may-23-2021) being recreated nearly every command.

//Apis.Discord._client.CurrentUser.Id
//apis.discord._client.getguild(gId).getuser(uId)

Provide a client variable as well as an easier way to get a user/guild/guilduser/channel

Simplify these:
//var msg = cea.Message.ToLower().Split(" ");
StringBuilder FavoriteCharacter = new StringBuilder(aca.Message).Replace(Command, "");
var hot = abd.Message.Replace($"{Command} ", "").Split(" or ");

{
StringBuilder FavoriteCharacter = new StringBuilder(a.Message.ToLower().Replace(Command.ToLower(), ""));

while (FavoriteCharacter[0] == ' ')
	FavoriteCharacter.Remove(0, 1);
while (FavoriteCharacter.ToString().Contains("  "))
	FavoriteCharacter.Replace("  ", " ");
while (FavoriteCharacter[^1] == ' ')
	FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
}

Favorite Character Selection Utilities

-The (fc) fc builder, which converts plain english to a gelbooru tag
-an fc breaker/splitter, or multi fc selection utilites

If other abbybots are in the same guild
if this message is in a server
if the channel is lewd

//a link to the channel, guild, and user

make it possible to directly check if a user has ratings "hasRatings(int 0), hasRatings((capi.Ratings)0)" etc

//a direct link to passiveusersql to decrease it's method arg count

//move the gelbooru image finding method into the command args

//aca.IsNSFW() is too broad of an idea that it becomes hard to know what is nsfw. Is it the user that's nsfw, or the channel, or both? does that mean the guild allows nsfw? That's what i mean by broad.

We need a prefix system, the abbybot command args are perfect for this

mentioned user ids, should have direct user info
- isMentioning
- getMentionUsersDiscord()
- getMentionUsersAbbybot()


idea:
var i = abd.Message.ToLower().Split(" ");
to
var i = abd.Split(" ");

idea:
aca.Message.Replace(Command, "")
to
aca.Replace(Command, "")

idea:
bool v = (aca.Message.ToLower().Contains(Command.ToLower()));
to
bool v = aca.Contains(Command, lower: true);

We will move the stringbuilder we make in every command directly into the abbybot command args

Another question:
The name "AbbybotCommandArgs" has grown old, what should I rename it to?