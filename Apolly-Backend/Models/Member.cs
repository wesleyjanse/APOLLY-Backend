using Apolly_Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_API.Models
{
    public class Member
    {
        public long MemberID{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        
        [NotMapped]
        public string Token { get; set; }

        //public ICollection<PollMember> PollMember { get; set; }

        //public ICollection<FriendRelationship> Friends { get; set; }

        //public ICollection<Vote> Votes { get; set; }
    }
}
