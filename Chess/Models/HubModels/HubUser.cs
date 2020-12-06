using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chess.Models.HubModels
{
    public class HubUser
    {
        // User identifier
        public string Name { get; set; }
        public HubUser Opponent { get; set; }
        public bool IsPlaying { get; set; }
        public string ConnectionId { get; set; }
    }
}