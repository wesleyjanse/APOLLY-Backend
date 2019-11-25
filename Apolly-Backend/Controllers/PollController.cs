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
    public class PollController : ControllerBase
    {
        private readonly ApollyContext _context;

        public PollController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/Poll
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
        {
            return await _context.Polls.Include(a => a.Answers).ThenInclude(a => a.Votes).ToListAsync();
        }

        // GET: api/Poll/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Poll>> GetPoll(long id)
        {
            var poll = await _context.Polls
                        .Include(i => i.Answers).ThenInclude(a => a.Votes)
                        .FirstOrDefaultAsync(i => i.PollID == id);

            if (poll == null)
            {
                return NotFound();
            }

            return poll;
        }

        // PUT: api/Poll/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(long id, Poll poll)
        {
            if (id != poll.PollID)
            {
                return BadRequest();
            }

            _context.Entry(poll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(id))
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

        // POST: api/Poll
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoll", new { id = poll.PollID }, poll);
        }

        // DELETE: api/Poll/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Poll>> DeletePoll(long id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            List<Answer> answers = _context.Answers.Where(a => a.PollID == id).ToList();
            poll.Answers = answers;

            foreach(PollMember p in _context.PollMembers.Where(p => p.PollID == id).ToList())
            {
                _context.PollMembers.Remove(p);
            }
            foreach(Answer a in answers)
            {
                _context.Answers.Remove(a);
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return poll;
        }

        private bool PollExists(long id)
        {
            return _context.Polls.Any(e => e.PollID == id);
        }
    }
}
