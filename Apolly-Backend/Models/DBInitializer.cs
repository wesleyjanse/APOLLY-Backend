using Apolly_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_API.Models
{
    public class DBInitializer
    {
        public static void Initialize(ApollyContext context)
        {
            context.Database.EnsureCreated();// Look for any verkiezingen.
            if (context.Members.Any())
            {
                return;   // DB has been seeded
            }

            context.Members.AddRange(
                new Member { Username = "test1", Email = "test1@apolly.com", Password = "test" },
                new Member { Username = "test2", Email = "test2@apolly.com", Password = "test" },
                new Member { Username = "Dimensions", Email = "dimensions@apolly.com", Password = "test" }
                );
            context.SaveChanges();

            context.Polls.AddRange(
                new Poll { Name = "Which movie is better?", Private = false, CreatedOn = DateTime.Now },
                new Poll { Name = "Is IT better then sports?", Private = false, CreatedOn = DateTime.Now }
                );
            context.SaveChanges();

            context.Answers.AddRange(
                new Answer { PossibleAnswer = "Lord of the rings", PollID = 1 },
                new Answer { PossibleAnswer = "The hobbit", PollID = 1 }
                );
            context.SaveChanges();

            context.PollMembers.AddRange(
                new PollMember { PollID = 1, MemberID = 1 },
                new PollMember { PollID = 2, MemberID = 1 }
                );
            context.SaveChanges();

            context.Votes.AddRange(
               new Vote { AnswerID = 1, MemberID = 1 }
               );
            context.SaveChanges();

            context.Friends.AddRange(
                new FriendRelationship { FriendID = 1, MemberID = 2}
               );
            context.SaveChanges();
        }
    }
}
