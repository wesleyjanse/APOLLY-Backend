using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apolly_Backend.Models
{
    [NotMapped]
    public class Email
    {
        public long emailID { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}
