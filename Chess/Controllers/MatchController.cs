using Chess.Bussiness;
using Chess.Bussiness.Services.Matches;
using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using Chess.Infrastructure.Hubs;
using Chess.Models.ViewModels.MatchViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Chess.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private readonly IMatchRepository matchRepository;
        private readonly IUserRepository userRepository;

        public MatchController(IMatchRepository matchRepo, IUserRepository userRepo)
        {
            matchRepository = matchRepo;
            userRepository = userRepo;
        }
        public ActionResult Index()
        {
            return View(userRepository.FindUserById(WebSecurity.CurrentUserId));
        }
        [HttpPost]
        public ActionResult Index(User model)
        {
            if (ModelState.IsValid)
            {
                userRepository.UpdateUser(model);
            }
            return RedirectToAction("Index", "Match");
        }

        [AllowAnonymous]
        public ActionResult Famous(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(matchRepository.Matches.Where(x => !x.IsPlayed).OrderBy(x => x.DateCreated).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MatchViewModel match)
        {
            if (ModelState.IsValid)
            {
                var newMatch = MatchViewModel.MapToMatch(match);
                newMatch.DateCreated = DateTime.Now;
                newMatch.IsPlayed = false;
                matchRepository.SaveMatch(newMatch);
            }
            return RedirectToAction("Famous");
        }

        public ActionResult Edit(int id)
        {
            var match = matchRepository.FindMatchById(id);
            if (match == null || match.IsPlayed)
                return HttpNotFound();
            return View(MatchViewModel.MapToMatchViewModel(match));
        }

        [HttpPost]
        public ActionResult Edit(MatchViewModel match)
        {
            if (ModelState.IsValid)
            {
                var newMatch = MatchViewModel.MapToMatch(match);
                newMatch.DateUpdated = DateTime.Now;
                matchRepository.UpdateMatch(newMatch);
            }
            return RedirectToAction("Famous");
        }

        public ActionResult Delete(int id)
        {
            var match = matchRepository.FindMatchById(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var match = matchRepository.FindMatchById(id);
            if (match != null)
            {
                matchRepository.DeleteMatch(match);
            }
            return RedirectToAction("Famous");
        }

        public ActionResult Match(int white, int black, int minutes, int gain, string identifier)
        {
            if (!MatchIdentifier.CheckMatchIdentifier(white, black, minutes, gain, identifier))
                return HttpNotFound();
            
            var match = new Match
            {
                White = userRepository.FindUserById(white),
                Black = userRepository.FindUserById(black),
                IsRated = true
            };
            ViewBag.Seconds = minutes * 60;
            ViewBag.Gain = gain;
            ViewBag.IsWhite = DateTime.Now.Ticks % 2 == 0;
            ViewBag.Identifier = identifier;
            return View(match);
        }

        [HttpPost]
        public void Match(int whiteId, int blackId, string result, string pieces, string moves, bool isRated)
        {
            var white = userRepository.FindUserById(whiteId);
            var black = userRepository.FindUserById(blackId);
            var match = new Match
            {
                White = white,
                Black = black,
                Comments = new List<Comment>(),
                Moves = MoveParser.ParseMatchMoves(moves, pieces),
                DateCreated = DateTime.Now,
                Result = result,
                IsPlayed = true,
                IsRated = isRated,
                Description = MatchService.GetDescription(white.UserName, black.UserName, result, "ChessMasters.com", DateTime.Now.ToShortDateString())
            };
            matchRepository.SaveMatch(match);
            CalculateRating(result, white, black);
        }

        private void CalculateRating(string result, User white, User black)
        {
            double scoreWhite = result.Equals("1/2") ? 0.5 : result.Equals("1 : 0") ? 1 : 0;
            double scoreBlack = result.Equals("1/2") ? 0.5 : result.Equals("1 : 0") ? 0 : 1;
            int whiteRating = white.PlayingRating;
            int blackRating = black.PlayingRating;
            white.PlayingRating += (int)RatingCalculation.CalculateElo(whiteRating, blackRating, scoreWhite);
            black.PlayingRating += (int)RatingCalculation.CalculateElo(whiteRating, blackRating, scoreBlack);
            userRepository.UpdateUser(white);
            userRepository.UpdateUser(black);
        }

        [AllowAnonymous]
        public ActionResult ViewMatch(int id)
        {
            var match = matchRepository.FindMatchById(id);
            ViewBag.Moves = MoveParser.StringBuildProblemMoves(match.Moves);
            return View(match);
        }

        [HttpPost]
        public ActionResult AddComment(int id, string comment)
        {
            var newComment = new Comment
            {
                Content = comment,
                DateCreated = DateTime.Now,
                Match = matchRepository.FindMatchById(id),
                User = userRepository.FindUserById(WebSecurity.CurrentUserId)
            };

            userRepository.AddUserComment(newComment);
            if (Request.IsAjaxRequest())
            {
                var returnData = new { newComment.User.UserName, newComment.Content, DateCreated = newComment.DateCreated.ToString() };
                return Json(returnData);
            }
            return RedirectToAction("ViewMatch", new { id = id });
        }

        public void DeleteComment(int commentId)
        {
            userRepository.DeleteUserComment(userRepository.GetUserComments(WebSecurity.CurrentUserId).FirstOrDefault(x => x.ID == commentId));
        }

        public ActionResult ListOnlineUsers()
        {
            var currentUser = userRepository.FindUserById(WebSecurity.CurrentUserId);
            var listUsers = new List<IndexViewModel>();
            var loggedInUsers = SparringHub.GetOnlineUsers();
            if (loggedInUsers != null && loggedInUsers.Count > 0)
            {
                //List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                foreach (var user in loggedInUsers)
                {
                    //var loggedUser = userRepository.FindUserByUserName(user);
                    if (user.PlayingRating >= currentUser.RatingFrom && user.PlayingRating <= currentUser.RatingTo && user.ID != currentUser.ID)
                        listUsers.Add(new IndexViewModel
                        {
                            User = user,
                            Identifier = MatchIdentifier.CalculateMatchIdentifier(currentUser.ID, user.ID, currentUser.Minutes, currentUser.Gain)
                        });
                }
            }
            return PartialView(listUsers);
        }
    }
}
