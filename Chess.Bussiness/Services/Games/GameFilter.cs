using Chess.Bussiness.Services.Enums;
using Chess.Domain.Entities;

namespace Chess.Bussiness.Services.Games
{
    public class GameFilter
    {
        /// <summary>
        /// Type of the game.
        /// </summary>
        public GameTypeEnum Type { get; set; }
        /// <summary>
        /// Ordering by the name of the game.
        /// </summary>
        public OrderByEnum Name { get; set; }
        /// <summary>
        /// Ordering by the code of the game.
        /// </summary>
        public OrderByEnum Code { get; set; }
        /// <summary>
        /// String used for searching by name.
        /// </summary>
        public string SearchName { get; set; }
    }
}
