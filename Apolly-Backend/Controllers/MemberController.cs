using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_API.Models;
using ASP.NET_Core_API.Services;
using System.Net.Mail;
using System.Net;
using Apolly_Backend.Models;
using Microsoft.AspNetCore.Authorization;

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


        // Authenticates user on login
        // Returns JWT token
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]Member userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET: api/Member
        // Get all members that are registered (Exlcuding members created by the back-end)
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.Where(m => m.Registered == true).ToListAsync();
        }

        // Send email to person who has been invited to a poll
        // Mailcient = Mailtrap
        [Authorize]
        [HttpPost]
        [Route("sendMail")]
        public void SendEmail(Email email)
        {
            MailAddress to = new MailAddress(email.email);
            MailAddress from = new MailAddress("Apolly@Invitations.com");

            MailMessage message = new MailMessage(from, to);
            message.Subject = email.subject;
            message.Body = email.message;
            message.IsBodyHtml = true;

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("c421f1707f4327", "2d7ec1af7d8fda"),
                EnableSsl = true
            };

            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Get member where username = x
        [Authorize]
        [HttpGet]
        [Route("getWhereName/{username}")]
        public async Task<ActionResult<Member>> getWhereName(string username)
        {
            return await _context.Members.Where(m => m.Username.ToLower() == username).FirstAsync();
        }

        // Get member where email = x
        [Authorize]
        [HttpGet]
        [Route("getWhereEmail/{email}")]
        public async Task<ActionResult<Member>> getWhereEmail(string email)
        {
            return await _context.Members.Where(m => m.Email.ToLower() == email).FirstAsync();
        }

        // GET: api/Member/5
        // Get member by id
        [Authorize]
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
        // Update member
        [Authorize]
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
        // Post new member
        [Authorize]
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
        // Delete member
        [Authorize]
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
