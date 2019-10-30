using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_API.Models;
using Apolly_Backend.Models;

namespace Apolly_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ApollyContext _context;

        public FriendController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/Friend
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendRelationship>>> GetFriends()
        {
            return await _context.Friends.ToListAsync();
        }

        // GET: api/Friend/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FriendRelationship>> GetFriendRelationship(long id)
        {
            var friendRelationship = await _context.Friends.FindAsync(id);

            if (friendRelationship == null)
            {
                return NotFound();
            }

            return friendRelationship;
        }

        // PUT: api/Friend/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendRelationship(long id, FriendRelationship friendRelationship)
        {
            if (id != friendRelationship.FriendRelationshipID)
            {
                return BadRequest();
            }

            _context.Entry(friendRelationship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendRelationshipExists(id))
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

        // POST: api/Friend
        [HttpPost]
        public async Task<ActionResult<FriendRelationship>> PostFriendRelationship(FriendRelationship friendRelationship)
        {
            _context.Friends.Add(friendRelationship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriendRelationship", new { id = friendRelationship.FriendRelationshipID }, friendRelationship);
        }

        // DELETE: api/Friend/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FriendRelationship>> DeleteFriendRelationship(long id)
        {
            var friendRelationship = await _context.Friends.FindAsync(id);
            if (friendRelationship == null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friendRelationship);
            await _context.SaveChangesAsync();

            return friendRelationship;
        }

        private bool FriendRelationshipExists(long id)
        {
            return _context.Friends.Any(e => e.FriendRelationshipID == id);
        }
    }
}
