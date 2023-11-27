using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Sem3ApiContext _context;
        private readonly IConfiguration _config;
        public AuthController(Sem3ApiContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenJWT(User user)
        {
            var secretkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(secretkey,
                                    SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role,"user"),

            };
            var token = new JwtSecurityToken(
                    _config["JWT:Issuer"],
                    _config["JWT:Audience"],
                    payload,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signatureKey
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpGet]
        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            List<UserDTO> data = new List<UserDTO>();
            foreach (User c in users)
            {
                data.Add(new UserDTO { id = c.Id, name = c.Name });
            }
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Register(UserRegister model)
        {
            try
            {
                var saft = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashed = BCrypt.Net.BCrypt.HashPassword(model.password, saft);
                var user = new User
                {
                    Email = model.email,
                    Name = model.name,
                    Phone = model.phone,
                    City = model.city,
                    Address = model.address,
                    Password = hashed
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(new UserDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    phone = user.Phone,
                    city = user.City,
                    address = user.Address,
                    token = GenJWT(user)
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin model)
        {
            try
            {
                var user = _context.Users.Where(u => u.Email.Equals(model.email))
                            .First();
                if (user == null)
                {
                    throw new Exception("Email or Password is not correct");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(model.password, user.Password);
                if (!verified)
                {
                    throw new Exception("Email or Password is not correct");
                }
                return Ok(new UserDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    phone = user.Phone,
                    city = user.City,
                    address = user.Address,
                    token = GenJWT(user)
                });

            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }


        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            // get info from token
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                return Unauthorized("Not Authorized");
            }
            try
            {
                var userClaims = identity.Claims;
                var userId = userClaims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = _context.Users.FirstOrDefault(c => c.Id ==  Convert.ToInt32(userId));
                return Ok(new UserDTO // đúng ra phải là UserProfileDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    phone = user.Phone,
                    city = user.City,
                    address = user.Address
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
    }

}
