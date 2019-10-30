using ASP.NET_Core_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    public class Vote
    {
        public long VoteID { get; set; }
        public long AnswerID { get; set; }
        public long MemberID { get; set; }
        public Member Member { get; set; }
        //public Answer Answer { get; set; }
    }
}
