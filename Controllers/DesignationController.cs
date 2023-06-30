using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prectice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prectice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly DBContext _context;

        public DesignationController(DBContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Designations>>> GetDesignation()
        {
            return await _context.Designation.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Designations>> GetDesignation(int id)
        {
            var designation = await _context.Designation.FindAsync(id);

            if (designation == null)
            {
                return NotFound();
            }

            return designation;
        }
        [HttpPost]
        public async Task<ActionResult<Designations>> Insert(Designations designations)
        {
            _context.Designation.Add(designations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDesignation", new { id = designations.Id }, designations);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Designations designations)
        {
            if(id != designations.Id)
            {
                return BadRequest();
            }
            _context.Entry(designations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesignationExists(id))
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var designation = await _context.Designation.FindAsync(id);
            if(designation == null)
            {
                return NotFound();
            }
            _context.Designation.Remove(designation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DesignationExists(int id)
        {
            return _context.Designation.Any(e => e.Id == id);
        }
    }
}
