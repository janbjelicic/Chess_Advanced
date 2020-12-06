using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Chess.Domain.Concrete
{
    public class UserSolutionRepository : IUserSolutionRepository
    {
        private readonly ChessDatabase context = new ChessDatabase();

        public IQueryable<UserSolution> UserSolutions
        {
            get { return context.UserSolutions.Include(x => x.User).Include(x => x.Problem); }
        }

        public void SaveUserSolution(UserSolution userSolution)
        {
            if (userSolution == null)
                return;
            context.UserSolutions.Add(userSolution);
            context.SaveChanges();
        }

        public void UpdateUserSolution(UserSolution userSolution)
        {
            if (userSolution == null)
                return;
            var oldUserSolution = context.UserSolutions.Find(userSolution.ID);
            if (oldUserSolution == null)
                return;
            oldUserSolution.CurrentMove = userSolution.CurrentMove;
            oldUserSolution.NumberOfAttempts = userSolution.NumberOfAttempts;
            oldUserSolution.Problem = userSolution.Problem;
            oldUserSolution.User = userSolution.User;
            oldUserSolution.IsSolved = userSolution.IsSolved;
            oldUserSolution.DateCreated = userSolution.DateCreated;
            oldUserSolution.DateUpdated = userSolution.DateUpdated;
            context.SaveChanges();
        }

        public void DeleteUserSolution(UserSolution userSolution)
        {
            if (userSolution == null)
                return;
            context.UserSolutions.Remove(userSolution);
            context.SaveChanges();
        }

        public UserSolution FindUserSolutionById(int id)
        {
            return context.UserSolutions.Find(id);
        }


        public UserSolution FindUserSolutionByIdentifiers(int userId, int positionId)
        {
            var userSolution = context.UserSolutions.Include(x => x.User).FirstOrDefault(x => x.User.ID == userId && x.Problem.ID == positionId);
            if (userSolution == null)
            {
                userSolution = new UserSolution
                {
                    NumberOfAttempts = 0,
                    CurrentMove = 0,
                    DateCreated = DateTime.Now,
                    User = context.Users.Find(userId),
                    Problem = context.Positions.Find(positionId)
                };
                context.UserSolutions.Add(userSolution);
                context.SaveChanges();
            }
            return userSolution;
        }
    }
}
