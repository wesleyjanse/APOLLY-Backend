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
    public class AnswerController : ControllerBase
    {
        private readonly ApollyContext _context;

        public AnswerController(ApollyContext context)
        {
            _context = context;
        }

        // GET: api/Answer
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            return await _context.Answers.ToListAsync();
        }

        // GET: api/Answer/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(long id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        // PUT: api/Answer/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(long id, Answer answer)
        {
            if (id != answer.AnswerID)
            {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answer
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.AnswerID }, answer);
        }

        // DELETE: api/Answer/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answer>> DeleteAnswer(long id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return answer;
        }

        private bool AnswerExists(long id)
        {
            return _context.Answers.Any(e => e.AnswerID == id);
        }
    }
}
