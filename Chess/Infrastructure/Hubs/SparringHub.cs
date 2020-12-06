using Chess.Bussiness;
using Chess.Domain.Concrete;
using Chess.Domain.Entities;
using Chess.Models.HubModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Infrastructure.Hubs
{
    public class SparringHub : Hub
    {
        private static object _syncRoot = new object();
        private static int counter = 1;

        private static readonly List<HubUser> users = new List<HubUser>();

        public void Spare(int whiteId, int blackId, int minutes, int gain)
        {
            var player = users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (player == null) 
                return;
            var opponent = users.FirstOrDefault(x => x.Name.Equals(blackId.ToString()));
            if (opponent == null)
                return;
            Clients.Client(opponent.ConnectionId).sparePartners(whiteId, blackId, minutes, gain, MatchIdentifier.CalculateMatchIdentifier(whiteId, blackId, minutes, gain));
        }

        /// <summary>
        /// Registering a new client will add the client to the current list of clients
        /// and save the connection id which will be used to communicate with the client.
        /// </summary>
        /// <param name="data">The player name</param>
        public void RegisterClient(string data)
        {
            lock (_syncRoot)
            {
                var user = users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (user == null)
                {
                    user = new HubUser { ConnectionId = Context.ConnectionId, Name = counter.ToString() };
                    users.Add(user);
                }
                counter++;
                user.IsPlaying = false;
            }

            SendStatsUpdate();
        }

        public override Task OnConnected()
        {
            return SendStatsUpdate();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var player = users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (player == null)
                return null;
            users.Remove(player);
            counter--;
            return SendStatsUpdate();
        }
         
        public Task SendStatsUpdate()
        {
            return Clients.All.refreshAmountOfPlayers(new { amountOfClients = users.Count });
        }

        public static List<User> GetOnlineUsers()
        {
            var databaseUsers = new List<User>();
            using(var db = new ChessDatabase())
            foreach (var user in users)
            {
                databaseUsers.Add(db.Users.Find(Int32.Parse(user.Name)));
            }
            return databaseUsers;
        }
    }
}