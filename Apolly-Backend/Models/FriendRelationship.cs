using ASP.NET_Core_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    public class Friends
    {
        public long FriendsID { get; set; }
        public long MemberID { get; set; }
        public long FriendID { get; set; }
        public bool Accepted { get; set; }

        [ForeignKey("FriendID")]
        public Member Friend { get; set; }
        [ForeignKey("MemberID")]
        public Member Member { get; set; }
    }
}
