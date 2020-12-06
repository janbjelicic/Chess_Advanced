using Chess.Bussiness.Services.Enums;
using Chess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Bussiness.Services.Games
{
    public class GameService
    {
        public static List<Game> FilterGames(IQueryable<Game> games, GameFilter filter)
        {
            if (filter == null)
                return games.ToList();
            switch (filter.Type)
            {
                case GameTypeEnum.Opening:
                    games = games.Where(x => x.Type == GameTypeEnum.Opening);
                    break;
                case GameTypeEnum.Ending:
                    games = games.Where(x => x.Type == GameTypeEnum.Ending);
                    break;
                case GameTypeEnum.Strategy:
                    games = games.Where(x => x.Type == GameTypeEnum.Strategy);
                    break;
                default:
                    break;
            }
            switch (filter.Code)
            {
                case OrderByEnum.Ascending:
                    games = games.OrderBy(x => x.Code);
                    break;
                case OrderByEnum.Descending:
                    games = games.OrderByDescending(x => x.Code);
                    break;
                default:
                    break;
            }
            switch (filter.Name)
            {
                case OrderByEnum.Ascending:
                    games = games.OrderBy(x => x.Name);
                    break;
                case OrderByEnum.Descending:
                    games = games.OrderByDescending(x => x.Name);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchName))
                games = games.Where(x => x.Name.Contains(filter.SearchName));
            return games.ToList();
        }
    }
}
