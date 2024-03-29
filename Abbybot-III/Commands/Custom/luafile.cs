﻿using Abbybot_III.Apis.Twitter.Core;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using MoonSharp.Interpreter;

using System.IO;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom
{
    [Capi.Cmd("luafile", 1, 1)]
    class luafile : BaseCommand
    {
        public override async Task DoWork(AbbybotCommandArgs message)
        {
            Script script = new Script(CoreModules.Preset_HardSandbox); //no io/sys calls allowed
            script.Options.DebugPrint = async s => await message.Send(s); //when print is used send message

            var asx = message.originalMessage.Attachments;
            foreach (var a in asx)
            {
                if (!a.Filename.Contains(".lua")) continue;
                string fileurl = "";
                try
                {
                    fileurl = await ImageDownloader.DownloadImage(a.Url);
                }
                catch
                {
                    await message.Send("I could tell it was a lua file but i couldn't quite get ahold of it... (download failed...)");
                }
                try
                {
                    var lua = File.ReadAllText(fileurl);
                    DynValue d = script.DoString(lua);
                }
                catch
                {
                    await message.Send("I could tell it was a lua file but i couldn't read it...");
                }
            }
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs cea)
        {
            var asx = cea.originalMessage.Attachments;
            bool oai = false;
            foreach (var a in asx)
            {
                if (a.Filename.Contains(".lua"))
                {
                    oai = true;
                    break;
                }
            }

			return oai && await base.Evaluate( cea );
		}

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return "I can run the lua files you upload into a server";
        }
    }
}