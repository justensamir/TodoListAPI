using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodosAPI.DTOS;
using TodosAPI.Models;

namespace TodosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IMapper _mapper, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            mapper = _mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<ApplicationUser>(registerDTO);
                user.PasswordHash = registerDTO.Password;
                user.UserName = user.Email;
                user.EmailConfirmed = true;
                IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);
                return result.Succeeded ? Ok() : BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginDTO.Email);
                if (user is not null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                    if (result.Succeeded)
                    {
                        #region Claims
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim("Name", $"{user.FirstName} {user.LastName}"),
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Email, user.Email)
                        };
                        #endregion

                        #region SecretKey
                        string key = "Oasis API Task .NET Developer";
                        byte[] encodedKey = Encoding.ASCII.GetBytes(key);
                        SecurityKey securityKey = new SymmetricSecurityKey(encodedKey);
                        #endregion

                        #region Generate Token
                        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        int tokenLifeDays = loginDTO.RememberMe ? 5 : 1;
                        var token = new JwtSecurityToken(
                            claims: claims,
                            signingCredentials: signingCredentials,
                            expires: DateTime.Now.AddDays(tokenLifeDays)
                            );
                        #endregion

                        #region Convert Token to String
                        var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
                        #endregion
                        await signInManager.SignInAsync(user, loginDTO.RememberMe);
                        return Ok(stringToken);
                    }
                }
            }
            return Unauthorized();
        }


    }
}
