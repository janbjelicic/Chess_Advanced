using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chess.Models.HubModels
{
    public class HubMatch
    {
        public string Identifier { get; set; }
        public string WhiteConnectionId { get; set; }
        public string BlackConnectionId { get; set; }
    }
}