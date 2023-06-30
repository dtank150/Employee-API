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
    public class EmployeeController : ControllerBase
    {
        private readonly DBContext _context;

        public EmployeeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var employees = (from e in _context.Employees
                             join d in _context.Designation
                             on e.DesignationID equals d.Id

                             select new Employee
                             {
                                 Id = e.Id,
                                 FirstName = e.FirstName,
                                 LastName = e.LastName,
                                 Email = e.Email,
                                 Age = e.Age,
                                DesignationID = e.DesignationID,
                                Designation = d.Designation,
                                 DOJ = e.DOJ,
                                 Gender = e.Gender,
                                 IsMarried = e.IsMarried,
                                 IsActive = e.IsActive
                             }).ToListAsync();
            return await employees;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            return employee;
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> Insert(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            if(id != employee.Id)
            {
                return BadRequest();
            }

            var existEmployee = await _context.Employees.FindAsync(id);
            if(existEmployee == null)
            {
                return NotFound();
            }
            existEmployee.FirstName = employee.FirstName;
            existEmployee.LastName = employee.LastName;
            existEmployee.Email = employee.Email;
            existEmployee.Age = employee.Age;
            existEmployee.DesignationID = employee.DesignationID;
            existEmployee.DOJ = employee.DOJ;
            existEmployee.Gender = employee.Gender;
            existEmployee.IsMarried = employee.IsMarried;
            existEmployee.IsActive = employee.IsActive;

            _context.Entry(existEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
