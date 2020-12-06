using Chess.Bussiness;
using Chess.Bussiness.Services.Enums;
using Chess.Bussiness.Services.Positions;
using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using Chess.Domain.Resources;
using Chess.Models.ViewModels.PositionViewModels;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Chess.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly IPositionRepository positionRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserSolutionRepository userSolutionRepository;

        public PositionController(IPositionRepository positionRepo, IUserRepository userRepo, IUserSolutionRepository userSolutionRepo)
        {
            positionRepository = positionRepo;
            userRepository = userRepo;
            userSolutionRepository = userSolutionRepo;
        }

        public ActionResult Index(PositionFilter filter, int? page)
        {
            if (filter != null)
                filter.UserID = WebSecurity.CurrentUserId;
            ViewBag.Difficulties = new SelectList(Enum.GetValues(typeof(DifficultyEnum)).Cast<DifficultyEnum>().ToList());
            ViewBag.Orders = new SelectList(Enum.GetValues(typeof(OrderByEnum)).Cast<OrderByEnum>().ToList());
            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(CategoryEnum)).Cast<CategoryEnum>().ToList());
            ViewBag.Bools = new SelectList(Enum.GetValues(typeof(BoolEnum)).Cast<BoolEnum>().ToList());
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(new IndexViewModel
            {
                Filter = filter,
                Positions = PositionDataViewModel.MapToPositionDataViewModels(
                                PositionService.FilterPositions(filter, positionRepository.Positions), WebSecurity.CurrentUserId)
                                .ToPagedList(pageNumber, pageSize)
            });
        }

        public ActionResult Create()
        {
            ViewBag.Difficulties = new SelectList(Enum.GetValues(typeof(DifficultyEnum)).Cast<DifficultyEnum>().ToList());
            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(CategoryEnum)).Cast<CategoryEnum>().ToList());
            return View();
        }

        [HttpPost]
        public ActionResult Create(PositionViewModel position)
        {
            if (ModelState.IsValid)
            {
                var newPosition = PositionViewModel.MapToPosition(position);
                newPosition.DateCreated = DateTime.Now;
                positionRepository.SavePosition(newPosition);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Difficulties = new SelectList(Enum.GetValues(typeof(DifficultyEnum)).Cast<DifficultyEnum>().ToList());
            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(CategoryEnum)).Cast<CategoryEnum>().ToList());
            return View(PositionViewModel.MapToPositionViewModel(positionRepository.FindPositionById(id)));
        }
        [HttpPost]
        public ActionResult Edit(PositionViewModel position)
        {
            if (ModelState.IsValid)
            {
                var newPosition = PositionViewModel.MapToPosition(position);
                newPosition.DateUpdated = DateTime.Now;
                positionRepository.UpdatePosition(newPosition);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var position = positionRepository.FindPositionById(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var position = positionRepository.FindPositionById(id);
            if (position != null)
            {
                positionRepository.DeletePosition(position);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Solve(int id)
        {
            return View(new SolveProblemModel
            {
                Position = positionRepository.FindPositionById(id),
                UserSolution = userSolutionRepository.FindUserSolutionByIdentifiers(WebSecurity.CurrentUserId, id)
            });
        }

        [HttpPost]
        public ActionResult AddComment(int id, string comment)
        {
            var newComment = new Comment
            {
                Content = comment,
                DateCreated = DateTime.Now,
                Position = positionRepository.FindPositionById(id),
                User = userRepository.FindUserById(WebSecurity.CurrentUserId)
            };

            userRepository.AddUserComment(newComment);
            if (Request.IsAjaxRequest())
            {
                var returnData = new { newComment.User.UserName, newComment.Content, DateCreated = newComment.DateCreated.ToString() };
                return Json(returnData);
            }
            return RedirectToAction("Solve", new { id = id });
        }

        public void DeleteComment(int commentId)
        {
            userRepository.DeleteUserComment(userRepository.GetUserComments(WebSecurity.CurrentUserId).FirstOrDefault(x => x.ID == commentId));
        }

        [HttpPost]
        public JsonResult GetSolution(int id)
        {
            var position = positionRepository.FindPositionById(id);
            if (position == null)
            {
                return Json(new { result = false, message = Resources.DefaultErrorMessage });
            }
            return Json(new { moves = MoveParser.StringBuildProblemMoves(position.Solution) });
        }

        [HttpPost]
        public JsonResult CheckMove(string move, int moveNumber, int id)
        {
            var position = positionRepository.FindPositionById(id);
            if (position == null)
            {                
                return Json(new { result = false, message = Resources.DefaultErrorMessage });
            }
            var userSolution = userSolutionRepository.FindUserSolutionByIdentifiers(WebSecurity.CurrentUserId, id);
            if (userSolution.IsSolved)
            {
                return Json(new { result = false, message = Resources.SolvedProblemMessage, attempt = string.Format(Resources.NumberOfAttemptsMessage, userSolution.NumberOfAttempts) });
            }
            
            if (CheckSolution.CheckMove(move, moveNumber, position.Solution))
            {
                userSolution.CurrentMove = moveNumber;
                userSolution.DateUpdated = DateTime.Now;
                userSolution.IsSolved = (moveNumber + 1) == position.Solution.Count;
                if (userSolution.IsSolved)
                {
                    userSolution.NumberOfAttempts++;
                    userSolution.User.ProblemRating = RatingCalculation.CalculateRatingProblem(userSolution.User.ProblemRating, (int)position.Difficulty - userSolution.NumberOfAttempts / 10);
                }
                userSolutionRepository.UpdateUserSolution(userSolution);
                return Json(new { result = true, message = userSolution.IsSolved ? Resources.SolvedProblemMessage : Resources.CorrectMoveMessage, attempt = string.Format(Resources.NumberOfAttemptsMessage, userSolution.NumberOfAttempts) });
            }
            else
            {                
                userSolution.DateUpdated = DateTime.Now;
                userSolution.NumberOfAttempts++;
                userSolutionRepository.UpdateUserSolution(userSolution);
                return Json(new { result = false, message = Resources.IncorrectMoveMessage, attempt = string.Format(Resources.NumberOfAttemptsMessage, userSolution.NumberOfAttempts) });
            }
        }
    }
}
