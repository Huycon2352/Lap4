using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_P11.Data;
using NT106_P11.Models;

namespace NT106_P11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/members
        // Endpoint này dùng để lấy tất cả các thành viên
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _context.Members.ToListAsync();
            return Ok(members);
        }

        // GET: api/members/5
        // Endpoint này dùng để lấy thành viên theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // PUT: api/members/5
        // Endpoint này dùng để cập nhật thông tin thành viên theo id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.Id)
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

        // DELETE: api/members/5
        // Endpoint này dùng để xóa thành viên theo id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostMember(Member member)
        {
            if (member == null)
            {
                return BadRequest("Member data is required.");
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
