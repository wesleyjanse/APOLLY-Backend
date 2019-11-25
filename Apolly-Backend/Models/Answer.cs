using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    public class Answer
    {
        [Key]
        public long AnswerID { get; set; }
        public string PossibleAnswer { get; set; }
        public long? PollID { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
