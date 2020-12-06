using Chess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Domain.Concrete
{
    public class ChessDatabase : DbContext
    {
        public ChessDatabase()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserSolution> UserSolutions { get; set; }
     }
}
