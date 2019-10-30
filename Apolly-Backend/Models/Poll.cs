using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    public class Poll
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long PollID { get; set; }
        public string Name { get; set;}
        public bool Private { get; set; }
        public DateTime CreatedOn { get; set; }

        //public ICollection<PollMember> PollMember { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
