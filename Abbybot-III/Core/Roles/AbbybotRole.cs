using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Abbybot_III.Core.Data.User
{
    public class AbbybotRole
    {
        public ulong role;
        public CommandRatings[] allowedRatings;

        public AbbybotRole(ulong role, CommandRatings[] allowedRatings)
        {
            this.role = role;
            this.allowedRatings = allowedRatings;
        }
    }
}
