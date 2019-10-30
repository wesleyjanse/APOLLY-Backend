using ASP.NET_Core_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    public class PollMember
    {
        public long PollMemberID { get; set; }
        public long PollID { get; set; }
        public long MemberID { get; set; }
        public Poll Poll { get; set; }
        public Member Member { get; set; }
    }
}
