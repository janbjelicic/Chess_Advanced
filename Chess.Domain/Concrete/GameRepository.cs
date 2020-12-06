using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using System.Linq;

namespace Chess.Domain.Concrete
{
    public class GameRepository : IGameRepository
    {
        private readonly ChessDatabase context = new ChessDatabase();

        public IQueryable<Game> Games
        {
            get { return context.Games; }
        }

        public void SaveGame(Game game)
        {
            if (game == null)
                return;
            context.Games.Add(game);
            context.SaveChanges();
        }

        public void UpdateGame(Game game)
        {
            if (game == null)
                return;
            var oldGame = context.Games.Find(game.ID);
            if (oldGame == null)
                return;
            oldGame.Name = game.Name;
            oldGame.Code = game.Code;
            oldGame.Description = game.Description;
            oldGame.DateUpdated = game.DateUpdated;
            oldGame.PiecePositions = game.PiecePositions;
            oldGame.Type = game.Type;
            context.SaveChanges();
        }

        public void DeleteGame(Game game)
        {
            if (game == null)
                return;
            context.Games.Remove(game);
            context.SaveChanges();
        }

        public Game FindGameById(int id)
        {
            return context.Games.Find(id);
        }
    }
}