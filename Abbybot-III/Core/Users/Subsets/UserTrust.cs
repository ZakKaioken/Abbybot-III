using System;
using System.Collections.Generic;
using System.Text;

namespace Abbybot_III.Core.Data.User.Subsets
{
    public class UserTrust
    {
        public int ActivityLevel;
        public int Love;

        public bool inTimeOut;
        public DateTime TimeOutEndDate;
        public string timeoutReason;
    }
}
