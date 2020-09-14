using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser SentTo { get; set; }
    }
}
