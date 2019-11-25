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
    public class VoteController : ControllerBase
    {
        private readonly ApollyContext _context;

        public VoteController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/Vote
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            return await _context.Votes.Include(v => v.Member).ToListAsync();
        }

        // GET: api/Vote/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(long id)
        {
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // PUT: api/Vote/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(long id, Vote vote)
        {
            if (id != vote.VoteID)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Vote
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(Vote vote)
        {
            var member = _context.Members.Where(m => m.MemberID == vote.MemberID).Single();
            vote.Member = member;

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVote", new { id = vote.VoteID }, vote);
        }

        // DELETE: api/Vote/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vote>> DeleteVote(long id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return vote;
        }

        private bool VoteExists(long id)
        {
            return _context.Votes.Any(e => e.VoteID == id);
        }
    }
}
