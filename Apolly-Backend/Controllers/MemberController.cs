using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_API.Models;
using ASP.NET_Core_API.Services;

namespace Apolly_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ApollyContext _context;
        private IUserService _userService;

        public MemberController(ApollyContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("authenticate")]
        //public async Task<ActionResult<Member>> PostMember(Member member)
        public IActionResult Authenticate([FromBody]Member userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.Include(m => m.Friends).ToListAsync();
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(long id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(long id, Member member)
        {
            if (id != member.MemberID)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            List<Member> allMembers = _context.Members.ToList();
            foreach (var item in allMembers)
            {
                if (item.Username == member.Username || item.Email == member.Email)
                {
                    return BadRequest();
                }
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberID }, member);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> DeleteMember(long id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return member;
        }

        private bool MemberExists(long id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }
    }
}
