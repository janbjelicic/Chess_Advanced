using Chess.Models.HubModels;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chess.Infrastructure.Hubs
{
    public class MatchHub : Hub
    {
        private static object _syncRoot = new object();
        private static List<HubMatch> matches = new List<HubMatch>();

        public void PlayMove(string source, string target, string piece, string opponentId)
        {
            Clients.Client(opponentId).playMove(source, target, piece);
        }

        public void RegisterForMatch(string identifier, string isWhite)
        {
            bool white = isWhite.Equals("True");
            if (string.IsNullOrWhiteSpace(identifier))
                return;
            var match = matches.FirstOrDefault(x => x.Identifier.Equals(identifier));
            if (match == null)
            {
                match = new HubMatch
                {
                    Identifier = identifier
                };
                if (white)
                    match.WhiteConnectionId = Context.ConnectionId;
                else
                    match.BlackConnectionId = Context.ConnectionId;
                matches.Add(match);
            }
            else
            {
                if (white && string.IsNullOrWhiteSpace(match.WhiteConnectionId))
                    match.WhiteConnectionId = Context.ConnectionId;
                else if (!white && string.IsNullOrWhiteSpace(match.BlackConnectionId))
                    match.BlackConnectionId = Context.ConnectionId;
                if (!string.IsNullOrWhiteSpace(match.WhiteConnectionId) && !string.IsNullOrWhiteSpace(match.BlackConnectionId))
                {
                    Clients.Client(match.WhiteConnectionId).setBlackConnectionId(new { ID = match.BlackConnectionId });
                    Clients.Client(match.BlackConnectionId).setWhiteConnectionId(new { ID = match.WhiteConnectionId });
                }
            }            
        }

        public override Task OnConnected()
        {
            return SendStatsUpdate();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return null;
        }

        public Task SendStatsUpdate()
        {
            return null;
        }
    }
}