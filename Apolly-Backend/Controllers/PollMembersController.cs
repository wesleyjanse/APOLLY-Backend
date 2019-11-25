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
    public class PollMembersController : ControllerBase
    {
        private readonly ApollyContext _context;

        public PollMembersController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/PollMembers
        // Returns only the unique pollmember objects. 
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollMember>>> GetPollMembers()
        {
            List<PollMember> pollMembers = await _context.PollMembers.Include(p => p.Member).Include(p => p.Poll).ThenInclude(p => p.Answers).ThenInclude(a => a.Votes).ThenInclude(v => v.Member).ToListAsync();
            List<long> pollIds = new List<long>();
            List<PollMember> pollMembersUnique = new List<PollMember>();
            foreach (var p in pollMembers)
            {
                if (!pollIds.Contains(p.PollID))
                {
                    pollMembersUnique.Add(p);
                }
                pollIds.Add(p.PollID);
            }

            return pollMembersUnique;
        }

        // GET: api/PollMembers/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PollMember>> GetPollMember(long id)
        {
            var pollMember = await _context.PollMembers.FindAsync(id);

            if (pollMember == null)
            {
                return NotFound();
            }

            return pollMember;
        }

        // GET: api/PollMembers/5
        [Authorize]
        [HttpGet]
        [Route("getAllByMemberId/{memberId}")]
        public async Task<ActionResult<IEnumerable<PollMember>>> GetAllPollMembersByMemberID(long memberId)
        {
            var pollMembers = await _context.PollMembers.Where(p => p.MemberID == memberId).Include(p => p.Member).Include(p => p.Poll).ThenInclude(p => p.Answers).ThenInclude(a => a.Votes).ThenInclude(v => v.Member).ToListAsync();

            if (pollMembers == null)
            {
                return NotFound();
            }

            return pollMembers;
        }
        [Authorize]
        [HttpGet]
        [Route("getCreatorByPollId/{pollId}")]
        public async Task<ActionResult<Member>> getCreatorByPollId(long pollId)
        {
            var pollMember = await _context.PollMembers.Where(p => p.PollID == pollId).Where(p => p.Creator == true).FirstAsync();
            var member = await _context.Members.Where(m => m.MemberID == pollMember.MemberID).FirstAsync();

            return member;
        }
        [Authorize]
        [HttpGet]
        [Route("getPollMemberByPollId/{pollId}")]
        public async Task<ActionResult<PollMember>> getPollMemberByPollId(long pollId)
        {
            var pollMember = await _context.PollMembers.Where(p => p.PollID == pollId).Include(p => p.Member).FirstOrDefaultAsync();

            if (pollMember == null)
            {
                return NotFound();
            }

            return pollMember;
        }

        // PUT: api/PollMembers/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollMember(long id, PollMember pollMember)
        {
            if (id != pollMember.PollMemberID)
            {
                return BadRequest();
            }

            _context.Entry(pollMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollMemberExists(id))
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

        // POST: api/PollMembers
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PollMember>> PostPollMember(PollMember pollMember)
        {
            _context.PollMembers.Add(pollMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollMember", new { id = pollMember.PollMemberID }, pollMember);
        }

        // DELETE: api/PollMembers/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PollMember>> DeletePollMember(long id)
        {
            var pollMember = await _context.PollMembers.FindAsync(id);
            if (pollMember == null)
            {
                return NotFound();
            }

            _context.PollMembers.Remove(pollMember);
            await _context.SaveChangesAsync();

            return pollMember;
        }

        private bool PollMemberExists(long id)
        {
            return _context.PollMembers.Any(e => e.PollMemberID == id);
        }
    }
}
