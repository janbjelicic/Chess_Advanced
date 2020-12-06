using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Chess.Domain.Concrete
{
    public class MatchRepository : IMatchRepository
    {
        private ChessDatabase context = new ChessDatabase();
        public IQueryable<Match> Matches
        {
            get
            {
                return
                    context.Matches.Include(x => x.Moves)
                        .Include(x => x.Comments)
                        .Include(x => x.White)
                        .Include(x => x.Black);
            }
        }

        public void SaveMatch(Match match)
        {
            if (match == null)
                return;
            if (match.White != null)
                match.White = context.Users.Find(match.White.ID);
            if (match.Black != null)
                match.Black = context.Users.Find(match.Black.ID);
            context.Matches.Add(match);
            context.SaveChanges();
        }

        public void UpdateMatch(Match match)
        {
            if (match == null)
                return;
            var updatedMatch = context.Matches.Include(x => x.Moves).FirstOrDefault(x => x.ID == match.ID);
            if (updatedMatch != null)
            {
                updatedMatch.Result = match.Result;
                updatedMatch.IsPlayed = match.IsPlayed;
                updatedMatch.DateUpdated = DateTime.Now;
                updatedMatch.IsRated = match.IsRated;
                updatedMatch.Black = match.Black;
                updatedMatch.Description = match.Description;
                updatedMatch.Comments = match.Comments;
                updatedMatch.Moves = match.Moves;
                updatedMatch.White = match.White;
            }
            context.SaveChanges();
        }

        public void DeleteMatch(Match match)
        {
            if (match == null)
                return;
            context.Matches.Remove(match);
            context.SaveChanges();
        }

        public Match FindMatchById(int id)
        {
            return context.Matches
                .Include(x => x.Moves)
                .Include(x => x.Comments)
                .Include(x => x.Comments.Select(y => y.User))
                .Include(x => x.White)
                .Include(x => x.Black).FirstOrDefault(x => x.ID == id);
        }
    }
}