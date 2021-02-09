using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1.Messages
{
    public class MessageToMom
    {
        public string From { get; set; }
        public DateTime? HowSoon { get; set; }
        public string Greeting { get; set; }
        public decimal HowMuch { get; set; }
        public List<string> Flattery { get; set; }

    }
}
