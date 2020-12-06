using Chess.Domain.Abstract;
using Chess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Chess.Domain.Concrete
{
    public class UserRepository : IUserRepository
    {
        private ChessDatabase context = new ChessDatabase();
        public IQueryable<User> Users
        {
            get { return context.Users.Include(x => x.Matches).Include(x => x.Comments); }
        }

        public void SaveUser(User user)
        {
            if (user == null)
                return;
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                return;
            var updatedUser = context.Users.Include(x => x.Comments).Include(x => x.Matches)
                                    .Include(x => x.UserSolutions).FirstOrDefault(x => x.ID == user.ID);
            if (updatedUser != null)
            {
                updatedUser.PlayingRating = user.PlayingRating;
                updatedUser.ProblemRating = user.ProblemRating;
                updatedUser.DateUpdated = DateTime.Now;
                updatedUser.UserName = user.UserName;
                updatedUser.CultureInfo = user.CultureInfo;
                updatedUser.Matches = user.Matches;
                updatedUser.Comments = user.Comments;
                updatedUser.IsRated = user.IsRated;
                updatedUser.Gain = user.Gain;
                updatedUser.Minutes = user.Minutes;
                updatedUser.RatingTo = user.RatingTo;
                updatedUser.RatingFrom = user.RatingFrom;
            }
            context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            if (user == null)
                return;
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public User FindUserById(int id)
        {
            return context.Users.Include(x => x.Matches).Include(x => x.Comments).FirstOrDefault(x => x.ID == id);
        }

        public User FindUserByUserName(string userName)
        {
            return context.Users.Include(x => x.Matches).Include(x => x.Comments).FirstOrDefault(x => x.UserName == userName);
        }

        public List<Comment> GetUserComments(int id)
        {
            var user = context.Users.Include(x => x.Comments).FirstOrDefault(x => x.ID == id);
            return user != null ? user.Comments : null;
        }

        public List<Match> GetUserMatches(int id)
        {
            var user = context.Users.Find(id);
            return user != null ? user.Matches : null;
        }

        public void AddUserComment(Comment comment)
        {
            if (comment == null)
                return;
            if (comment.Match != null)
                comment.Match = context.Matches.Find(comment.Match.ID);
            if (comment.Position != null)
                comment.Position = context.Positions.Find(comment.Position.ID);
            comment.User = context.Users.Find(comment.User.ID);
            context.Comments.Add(comment);
            context.SaveChanges();
        }

        public void UpdateUserComment(Comment comment)
        {
            if (comment == null)
                return;
            var oldComment = context.Comments.Include(x => x.Match).Include(x => x.Position).Include(x => x.User).FirstOrDefault(x => x.ID == comment.ID);
            if (oldComment == null)
                return;
            oldComment.User = comment.User;
            oldComment.Position = comment.Position;
            oldComment.Match = comment.Match;
            oldComment.DateUpdated = comment.DateUpdated;
            oldComment.DateCreated = comment.DateCreated;
            oldComment.Content = comment.Content;
            context.SaveChanges();
        }

        public void DeleteUserComment(Comment comment)
        {
            if (comment == null)
                return;
            context.Comments.Remove(comment);
            context.SaveChanges();
        }        
    }
}