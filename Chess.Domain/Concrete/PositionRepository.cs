using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Chess.Domain.Concrete
{
    public class PositionRepository : IPositionRepository
    {
        private ChessDatabase context = new ChessDatabase();
        public IQueryable<Position> Positions
        {
            get
            {
                return context.Positions
                    .Include(x => x.Comments)
                    .Include(x => x.Solution)
                    .Include(x => x.UserSolutions)
                    .Include(x => x.Comments.Select(y => y.User))
                    .Include(x => x.UserSolutions.Select(y => y.User));
            }
        }

        public void SavePosition(Position position)
        {
            if (position == null)
                return;
            context.Positions.Add(position);
            context.SaveChanges();
        }

        public void UpdatePosition(Position position)
        {
            if (position == null)
                return;
            var updatedPosition = context.Positions.Include(x => x.Comments).Include(x => x.Solution)
                                    .Include(x => x.UserSolutions).FirstOrDefault(x => x.ID == position.ID);
            if (updatedPosition != null)
            {
                updatedPosition.Solution = position.Solution;
                updatedPosition.Name = position.Name;
                updatedPosition.Difficulty = position.Difficulty;
                updatedPosition.Description = position.Description;
                updatedPosition.PiecePositions = position.PiecePositions;
                updatedPosition.Comments = position.Comments;
                updatedPosition.DateUpdated = DateTime.Now;
                updatedPosition.Category = position.Category;
                updatedPosition.WhiteIsPlaying = position.WhiteIsPlaying;
                updatedPosition.UserSolutions = position.UserSolutions;
            }
            context.SaveChanges();
        }

        public void DeletePosition(Position position)
        {
            if (position == null)
                return;
            var oldPosition = context.Positions.Include(x => x.UserSolutions).FirstOrDefault(x=>x.ID == position.ID);
            if (oldPosition != null)
            {
                context.Positions.Remove(oldPosition);
                context.SaveChanges();
            }
        }

        public Position FindPositionById(int id)
        {
            return context.Positions
                .Include(x => x.Comments)
                .Include(x => x.Solution)
                .Include(x => x.Comments.Select(y => y.User))
                .FirstOrDefault(x => x.ID == id);
        }
    }
}