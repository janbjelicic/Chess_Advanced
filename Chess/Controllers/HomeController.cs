using Chess.Bussiness.Services.Users;
using Chess.Domain.Abstract;
using Chess.Filters;
using Chess.Models.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chess.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        public HomeController(IUserRepository userRepo) {
            userRepository = userRepo;
        }

        public ActionResult Index()
        {
            var users = userRepository.Users.ToList();
            return View(new IndexViewModel
            {
                BestPlayers = UserService.GetBestPlayers(users, 10),
                BestProblemSolvers = UserService.GetBestProblemSolvers(users, 10)
            });
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Cites()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
