using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;

namespace Auth.Controller
{
    [ApiController]


    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;

        }


        [HttpPost("admin/signup")]
        public async Task<IActionResult> saveAdmin([FromBody] AdminModel adminobj)
        {
            if (adminobj == null)
            {
                return BadRequest();
            }
            if (await CheckEmailExistAdmin(adminobj.email)) return BadRequest(new { Message = "Email Already Exist!!! " });
            if (await CheckEmailExistUser(adminobj.email)) return BadRequest(new { Message = "Email Already Exist!!!" });
            if (await CheckMobileExistUser(adminobj.mobileNumber)) return BadRequest(new { Message = "Mobile Number Already Exist!!! " });
            if (await CheckMobileExistAdmin(adminobj.mobileNumber)) return BadRequest(new { Message = "Mobile number  Already Exist!!! " });
            await _context.Admin.AddAsync(adminobj);
            await _context.SaveChangesAsync();
            var admin = new AdminModel
            {
                email = adminobj.email,
                password = adminobj.password,
                mobileNumber = adminobj.mobileNumber,
                userRole = adminobj.userRole
            };
            await _context.Admin.AddAsync(admin);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Admin added"
            });
        }
        [HttpPost("user/signup")]
        public async Task<IActionResult> saveUser([FromBody] UserModel userobj)
        {
            if (userobj == null)
            {
                return BadRequest();
            }
            if (await CheckEmailExistAdmin(userobj.email)) return BadRequest(new { Message = "Email Already Exist!!! " });
            if (await CheckEmailExistUser(userobj.email)) return BadRequest(new { Message = "Email Already Exist!!! " });
            if (await CheckMobileExistUser(userobj.mobileNumber)) return BadRequest(new { Message = "Mobile Number Already Exist!!! " });
            if (await CheckMobileExistAdmin(userobj.mobileNumber)) return BadRequest(new { Message = "Mobile number  Already Exist!!! " });
            await _context.User.AddAsync(userobj);
            await _context.SaveChangesAsync();
            var loginObj = new LoginModel
            {
                email = userobj.email,
                password = userobj.password
            };
            await _context.Login.AddAsync(loginObj);
            await _context.SaveChangesAsync();
            return Created("", true);
        }

        private Task<bool> CheckEmailExistUser(string Email)
        {
            return (_context.User.AnyAsync(x => x.email == Email));
        }
        private Task<bool> CheckEmailExistAdmin(string Email)
        {
            return (_context.Admin.AnyAsync(x => x.email == Email));
        }
        private Task<bool> CheckMobileExistUser(string mobile )
        {
            return (_context.User.AnyAsync(x => x.mobileNumber == mobile));
        }
        private Task<bool> CheckMobileExistAdmin(string mobile)
        {
            return (_context.User.AnyAsync(x => x.mobileNumber == mobile));
        }
    }

}