using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Capstone.Models.Common.Enums;
using Capstone.Models.Entities;
using Capstone.Persistence.Data;
using Capstone.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Capstone.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, AppDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel userRegistrationModel)
        {
            var existingUser = await _userManager.FindByNameAsync(userRegistrationModel.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User Already Exists." });
            }

            var user = new ApplicationUser()
            {
                UserName = userRegistrationModel.UserName,
                FirstName = userRegistrationModel.FirstName,
                LastName = userRegistrationModel.LastName,
                UserType = userRegistrationModel.userTypes,
                Email = userRegistrationModel.UserName,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);

            if (result.Succeeded)
            {
                switch (user.UserType)
                {
                    case UserTypes.Farmer:
                        var farmerdet = new user()
                        {
                            Name = user.FirstName+" "+user.LastName,
                            FarmLocation = user.Address,
                        };
                        await _context.Farmers.AddAsync(farmerdet);
                        break;

                     case UserTypes.ShopOwner:
                        var shopdet = new Shop()
                        {
                            Name = user.FirstName + " " + user.LastName,
                            Location = user.Address,
                        };
                        await _context.Shops.AddAsync(shopdet);
                        break;

                    default:
                        return BadRequest(new { message = "Invalid User Type." });
                }

                await _context.SaveChangesAsync();

                // Generate JWT Token
                JwtSecurityToken token = await GenerateToken(user);

                var response = new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    FirstName = user.FirstName,
                    UserID = user.Id,
                    Username = user.UserName,
                };

                return Ok(response);
            }
            return BadRequest(new { result.Errors });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userloginmodel) 
        {
            var existingUser = await _userManager.FindByNameAsync(userloginmodel.UserName);
            if (existingUser == null)
            {
                return BadRequest(new { message = "User Doesn't Exist." });
            }

            var result = await _signInManager.PasswordSignInAsync(userloginmodel.UserName,userloginmodel.Password,false,false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            JwtSecurityToken token = await GenerateToken(existingUser);

            var response = new LoginResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                FirstName = existingUser.FirstName,
                UserID = existingUser.Id,
                Username = existingUser.UserName,
            };
            return Ok(response);


        }


        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            string secretKey = "84321DFB66934ECC86D547C5CF5B3EAV";

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim("UserEmail", user.UserName),
                new Claim("Role", user.UserType.ToString()),
                new Claim("UserId", user.Id.ToString()),
            };


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "PlantManagementIssuer",
                audience: "PlantManagementAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
