using Apolly_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_API.Models
{
    public class ApollyContext: DbContext
    {
        public ApollyContext(DbContextOptions<ApollyContext> options) : base(options) { }
        public DbSet<Member> Members { get; set; }
        public DbSet<Poll> Polls { get; set; }

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<PollMember> PollMembers { get; set; }
        public DbSet<Friends> Friends { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Poll>().ToTable("Poll");
            modelBuilder.Entity<Vote>().ToTable("Vote");
            modelBuilder.Entity<Answer>().ToTable("Answer");
            modelBuilder.Entity<PollMember>().ToTable("PollMember");
            modelBuilder.Entity<Friends>().ToTable("Friends");
        }
    }
}
 