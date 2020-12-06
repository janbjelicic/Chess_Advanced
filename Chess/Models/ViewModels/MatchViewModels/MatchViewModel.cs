using Chess.Bussiness;
using Chess.Bussiness.Services.Matches;
using Chess.Domain.Entities;
using Chess.Domain.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Chess.Models.ViewModels.MatchViewModels
{
    public class MatchViewModel
    {
        /// <summary>
        /// Match identifier.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the white player.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "White")]
        [Required]
        public string WhiteName { get; set; }
        /// <summary>
        /// Name of the black player.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Black")]
        [Required]
        public string BlackName { get; set; }
        /// <summary>
        /// Place where the match was played.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Place")]
        public string Place { get; set; }
        /// <summary>
        /// Date when the match was played.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Date")]
        public DateTime DatePlayed { get; set; }
        /// <summary>
        /// Match result.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Result")]
        [Required]
        public string Result { get; set; }
        /// <summary>
        /// Moves played.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Moves")]
        public string Moves { get; set; }

        /// <summary>
        /// Mapping method from view model to entity model.
        /// </summary>
        /// <param name="match">Object that will be mapped.</param>
        /// <returns>New mapped object.</returns>
        public static Match MapToMatch(MatchViewModel match)
        {
            return new Match
            {
                ID = match.ID,
                DateCreated = match.DatePlayed,
                Description = MatchService.GetDescription(match.WhiteName, match.BlackName, match.Result, match.Place, match.DatePlayed.ToShortDateString()),
                Result = match.Result,
                Moves = MoveParser.ParseMatchMoves(match.Moves, "")
            };
        }
        /// <summary>
        /// Mapping method from entity model to view model.
        /// </summary>
        /// <param name="match">Object that will be mapped.</param>
        /// <returns>New mapped object.</returns>
        public static MatchViewModel MapToMatchViewModel(Match match)
        {
            var matchViewModel = SetDataFromDescription(match);
            matchViewModel.Moves = MoveParser.StringBuildMatchMoves(match.Moves);
            return matchViewModel;
        }

        /// <summary>
        /// Set the match data from the existing match description.
        /// </summary>
        /// <param name="match">Match from which description we fill the rest of the data.</param>
        private static MatchViewModel SetDataFromDescription(Match match)
        {
            var matchViewModel = new MatchViewModel();
            if (match == null || string.IsNullOrWhiteSpace(match.Description))
                return null;
            var parts = match.Description.Split(',');

            // Parsing the first part of the description
            var firstParts = parts[0].Trim().Split('-');
            var blackNameAndResult = firstParts[1].Trim().Split(' ');
            matchViewModel.WhiteName = firstParts[0].Trim();
            matchViewModel.BlackName = blackNameAndResult[0].Trim();
            matchViewModel.Result = blackNameAndResult[1].Trim();

            // Parsing the second part of the description
            var restOfData = parts[1].Trim().Split(' ');
            matchViewModel.Place = restOfData[0].Trim();
            matchViewModel.DatePlayed = DateTime.Parse(restOfData[1].Trim());
            return matchViewModel;
        }
    }
}