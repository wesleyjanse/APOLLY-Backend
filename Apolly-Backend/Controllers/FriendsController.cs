using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_API.Models;
using Apolly_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace Apolly_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly ApollyContext _context;

        public FriendsController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/Friends
        // Get all friends with includes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friends>>> GetFriends()
        {
            return await _context.Friends.Include(f => f.Friend).Include(f => f.Member).ToListAsync();
        }

        // GET: api/Friends
        // Get all friendobjects where memberID = x
        [Authorize]
        [HttpGet]
        [Route("getAllByMemberID/{memberID}")]
        public async Task<ActionResult<IEnumerable<Friends>>> getAllByMemberID(long memberID)
        {
            return await _context.Friends.Where(f => f.MemberID == memberID || f.FriendID == memberID).Include(f => f.Friend).Include(f => f.Member).OrderBy(f => f.Accepted).ToListAsync();
        }

        // Get all friendrequest where memberID = x 
        [Authorize]
        [HttpGet]
        [Route("getAllRequestsByMemberID/{memberID}")]
        public async Task<ActionResult<IEnumerable<Friends>>> getAllRequestsByMemberID(long memberID)
        {
            return await _context.Friends.Where(f => f.FriendID == memberID).Where(f => f.Accepted == false).Include(f => f.Friend).Include(f => f.Member).ToListAsync();
        }

        // Get the count of open Friendrequests and PollInvitations
        // Returns int
        [Authorize]
        [HttpGet]
        [Route("getCountNotifications/{memberID}")]
        public int getCountNotifications(long memberID)
        {
            int friendRequests = _context.Friends.Where(f => f.FriendID == memberID).Where(f => f.Accepted == false).Count();
            int pollRequests = _context.PollMembers.Where(p => p.MemberID == memberID).Where(p => p.Accepted == false).Count();
            int aantal = friendRequests + pollRequests;
            return aantal;
        }

        // GET: api/Friends/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Friends>> GetFriends(long id)
        {
            var friends = await _context.Friends.FindAsync(id);

            if (friends == null)
            {
                return NotFound();
            }

            return friends;
        }

        // PUT: api/Friends/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriends(long id, Friends friends)
        {
            if (id != friends.FriendsID)
            {
                return BadRequest();
            }

            _context.Entry(friends).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Friends
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Friends>> PostFriends(Friends friends)
        {
            _context.Friends.Add(friends);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriends", new { id = friends.FriendsID }, friends);
        }

        // DELETE: api/Friends/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friends>> DeleteFriends(long id)
        {
            var friends = await _context.Friends.FindAsync(id);
            if (friends == null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friends);
            await _context.SaveChangesAsync();

            return friends;
        }

        private bool FriendsExists(long id)
        {
            return _context.Friends.Any(e => e.FriendsID == id);
        }
    }
}
