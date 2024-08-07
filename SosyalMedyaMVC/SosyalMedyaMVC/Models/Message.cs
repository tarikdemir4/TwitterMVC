using System;

namespace SosyalMedyaMVC.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; } 
    }
}
