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
                new Member { Username = "Test1", Email = "Test1@apolly.com", Password = "test", Registered = true },
                new Member { Username = "Test2", Email = "Test2@apolly.com", Password = "test", Registered = true },
                new Member { Username = "Dimensions", Email = "Dimensions@apolly.com", Password = "test", Registered = true },
                new Member { Username = "Jose", Email = "Jose@apolly.com", Password = "test", Registered = true },
                new Member { Username = "Indy", Email = "IndyHuysmans@gmail.com", Password = "test", Registered = true },
                new Member { Username = "Jelle_VLD", Email = "Jelle-VLD@hotmail.be", Password = "test", Registered = true }
                );
            context.SaveChanges();

            context.Polls.AddRange(
                new Poll { Name = "Which movie is better?", Private = false, CreatedOn = DateTime.Now },
                new Poll { Name = "What is your favorite F1-Team?", Private = false, CreatedOn = DateTime.Now }
                );
            context.SaveChanges();

            context.Answers.AddRange(
                new Answer { PossibleAnswer = "Lord of the rings", PollID = 1 },
                new Answer { PossibleAnswer = "The hobbit", PollID = 1 },
                new Answer { PossibleAnswer = "Ferrari", PollID = 2 },
                new Answer { PossibleAnswer = "Redbull Racing", PollID = 2 },
                new Answer { PossibleAnswer = "Mercedes AMG Petronas", PollID = 2 }
                );
            context.SaveChanges();

            context.PollMembers.AddRange(
                new PollMember { PollID = 1, MemberID = 4, Accepted = true, Creator = true},
                new PollMember { PollID = 2, MemberID = 6, Accepted = true, Creator = true },
                new PollMember { PollID = 2, MemberID = 3, Accepted = false, Creator = false }
                );
            context.SaveChanges();

            context.Votes.AddRange(
               new Vote { AnswerID = 1, MemberID = 1 }
               );
            context.SaveChanges();
        }
    }
}
