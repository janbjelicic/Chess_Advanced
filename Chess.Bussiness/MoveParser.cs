using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Domain.Entities;

namespace Chess.Bussiness
{
    public class MoveParser
    {
        /// <summary>
        /// Parsing moves into entity objects.
        /// </summary>
        /// <param name="moves">Moves in the string format.</param>
        /// <returns>List of moves.</returns>
        public static List<Move> ParseProblemMoves(string moves)
        {
            var listMoves = new List<Move>();
            var moveCounter = 0;
            foreach (var move in moves.Split(',').ToList())
            {
                listMoves.Add(new Move
                {
                    Piece = PieceEnum.Unknown,
                    WhitePlayed = moveCounter % 2 == 0,
                    Content = move.Trim(),
                    MoveNumber = 1 + moveCounter / 2
                });
                moveCounter++;
            }
            return listMoves;
        }

        /// <summary>
        /// Building the moves string from list of entity objects.
        /// </summary>
        /// <param name="moves">Moves that will be connected into string.</param>
        /// <returns>Moves in a string format.</returns>
        public static string StringBuildProblemMoves(List<Move> moves)
        {
            return string.Join(", ", moves.Select(x => x.Content));
        }

        /// <summary>
        /// Method used for moves parsing.
        /// </summary>
        /// <param name="moves">Moves in string like format: e2-e4, e7-e5...</param>
        /// <param name="pieces">Pieces that were played on certain moves.</param>
        /// <returns>List of moves in c# object format.</returns>
        public static List<Move> ParseMatchMoves(string moves, string pieces)
        {
            var listMoves = new List<Move>();
            if (string.IsNullOrWhiteSpace(moves))
                return listMoves;
            var splittedMoves = moves.Trim().Split(' ');
            var splittedPieces = pieces.Split(' ');
            var numberOfMoves = splittedMoves.Length;

            var moveNumber = 0;
            for (int i = 0; i < numberOfMoves; i++)
            {
                var piece = PieceEnum.Unknown;
                if(splittedPieces != null && i < splittedPieces.Length)
                    piece = SetPiece(splittedPieces[i]);
                listMoves.Add(new Move
                {
                    Piece = piece,
                    Content = splittedMoves[i],
                    WhitePlayed = moveNumber % 2 == 0,
                    MoveNumber = moveNumber / 2 + 1
                });
                moveNumber++;
            }
            return listMoves;
        }        

        /// <summary>
        /// Building the moves string from list of entity objects.
        /// </summary>
        /// <param name="moves">Moves that will be connected into string.</param>
        /// <returns>Moves in a string format.</returns>
        public static string StringBuildMatchMoves(List<Move> moves)
        {
            return string.Join(" ", moves.Select(x => x.Content));
        }

        /// <summary>
        /// Determines which piece is played.
        /// </summary>
        /// <param name="piece">Piece signature in a string format.</param>
        /// <returns>Enum representation of the piece played.</returns>
        private static PieceEnum SetPiece(string piece)
        {
            switch (piece)
            {
                case "wP":
                case "bP":
                    return PieceEnum.Pawn;
                case "wN":
                case "bN":
                    return PieceEnum.Knight;
                case "wB":
                case "bB":
                    return PieceEnum.Bishop;
                case "wR":
                case "bR":
                    return PieceEnum.Rook;
                case "wQ":
                case "bQ":
                    return PieceEnum.Queen;
                case "wK":
                case "bK":
                    return PieceEnum.King;
                default:
                    return PieceEnum.Unknown;
            }
        }


    }
}
