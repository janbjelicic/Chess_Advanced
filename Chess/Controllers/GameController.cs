using Chess.Bussiness.Services.Enums;
using Chess.Bussiness.Services.Games;
using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chess.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;
        public GameController(IGameRepository gameRepo)
        {
            gameRepository = gameRepo;
        }

        [AllowAnonymous]
        public ActionResult Openings(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            var filter = GetGameFilter(sortOrder, searchString);
            filter.Type = GameTypeEnum.Opening;
            
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParameter = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.CodeSortParameter = sortOrder == "code_asc" ? "code_desc" : "code_asc";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.ReturnUrl = "Openings";
            return View("Index", GameService.FilterGames(gameRepository.Games, filter).ToPagedList(pageNumber, pageSize));
        }        

        [AllowAnonymous]
        public ActionResult Strategies(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            var filter = GetGameFilter(sortOrder, searchString);
            filter.Type = GameTypeEnum.Strategy;

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParameter = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.CodeSortParameter = sortOrder == "code_asc" ? "code_desc" : "code_asc";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.ReturnUrl = "Strategies";
            return View("Index", GameService.FilterGames(gameRepository.Games, filter).ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        public ActionResult Endings(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            var filter = GetGameFilter(sortOrder, searchString);
            filter.Type = GameTypeEnum.Ending;

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParameter = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.CodeSortParameter = sortOrder == "code_asc" ? "code_desc" : "code_asc";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.ReturnUrl = "Endings";
            return View("Index", GameService.FilterGames(gameRepository.Games, filter).ToPagedList(pageNumber, pageSize));
        }        

        public ActionResult Create(string returnUrl)
        {
            ViewBag.Types = new SelectList(Enum.GetValues(typeof(GameTypeEnum)).Cast<GameTypeEnum>().ToList());
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Game game, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                game.DateCreated = DateTime.Now;
                gameRepository.SaveGame(game);
            }
            return RedirectToAction(returnUrl);
        }

        public ActionResult Edit(int id, string returnUrl)
        {
            ViewBag.Types = new SelectList(Enum.GetValues(typeof(GameTypeEnum)).Cast<GameTypeEnum>().ToList());
            ViewBag.ReturnUrl = returnUrl;
            return View(gameRepository.FindGameById(id));
        }
        [HttpPost]
        public ActionResult Edit(Game game, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                game.DateUpdated = DateTime.Now;
                gameRepository.UpdateGame(game);
            }
            return RedirectToAction(returnUrl);
        }

        [AllowAnonymous]
        public ActionResult Details(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(gameRepository.FindGameById(id));
        }

        public ActionResult Delete(int id, string returnUrl)
        {
            var game = gameRepository.FindGameById(id);
            ViewBag.ReturnUrl = returnUrl;
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var game = gameRepository.FindGameById(id);
            if (game != null)
            {
                gameRepository.DeleteGame(game);
            }
            return RedirectToAction(returnUrl);
        }

        #region Helper Methods
        /// <summary>
        /// Gets game filter with parameters set according to the 
        /// set sort order parameter.
        /// </summary>
        /// <param name="sortOrder">Determines the ordering of the data.</param>
        /// <param name="searchString">String used for searching by name.</param>
        /// <returns>Filter with appropriate sorting data.</returns>
        private GameFilter GetGameFilter(string sortOrder, string searchString)
        {
            var filter = new GameFilter
            {
                SearchName = searchString
            };
            switch (sortOrder)
            {
                case "name_asc":
                    filter.Name = OrderByEnum.Ascending;
                    filter.Code = OrderByEnum.None;
                    return filter;
                case "name_desc":
                    filter.Name = OrderByEnum.Descending;
                    filter.Code = OrderByEnum.None;
                    return filter;
                case "code_asc":
                    filter.Name = OrderByEnum.None;
                    filter.Code = OrderByEnum.Ascending;
                    return filter;
                case "code_desc":
                    filter.Name = OrderByEnum.None;
                    filter.Code = OrderByEnum.Descending;
                    return filter;
                default:
                    filter.Name = OrderByEnum.None;
                    filter.Code = OrderByEnum.None;
                    return filter;
            }
        }
        #endregion
    }
}
