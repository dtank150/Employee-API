using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prectice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prectice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DBContext _context;

        public UserController(IConfiguration config, DBContext context) 
        {
            _config = config;
            _context = context;

        }

        [HttpPost("Register")]
        public IActionResult Create(User user)
        {
            if(_context.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return Ok("Alredy Exists");
            }
            user.MemberSince = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Success");
        }
        [HttpPost("Login")]
        public IActionResult Login(Login login)
        {
            var availble = _context.Users.Where(u => u.Email == login.Email).FirstOrDefault();
            if(availble != null)
            {
                return Ok("Success");
            }
            return Ok("Failure");
        }
    }   
}
