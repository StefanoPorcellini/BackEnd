//using Esercizio_Gestione_Albergo.Models.Login;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.IdentityModel.JsonWebTokens;
//using System.Security.Claims;
//using Esercizio_Gestione_Albergo.Services.Auth;

//namespace Esercizio_Gestione_Albergo.Controllers
//{
//    [Route("api/auth")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        private readonly IAuthService _authService;
//        private readonly string _issuer;
//        private readonly string _audience;
//        private readonly byte[] _key;
//        public AuthenticationController(
//            IAuthService authService,
//            IConfiguration configuration) {
//            _authService = authService;
//            _issuer = configuration["Jwt:Issuer"]!;
//            _audience = configuration["Jwt:Audience"]!;
//            _key = System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {   
//            var user = await _authService.GetUtenteAsync(model.Username, model.Password);
//            if (user == null) return Unauthorized();
            
//            var claims = new[] {
//                    new Claim(JwtRegisteredClaimNames.Name, model.Username),
//                    new Claim(JwtRegisteredClaimNames.Sub, model.Username),
//                    new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString()),
//                    new Claim("UserId", user.UserId.ToString())
//            };
//                var key = new SymmetricSecurityKey(_key);
//                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//                var expiration = DateTime.Now.AddYears(1);
//                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
//                    issuer: _issuer,
//                    audience: _audience,
//                    claims: claims,
//                    expires: expiration,
//                    signingCredentials: creds);
//                return Ok(new LoginResponseModel
//                {
//                    Username = model.Username,
//                    Expires = expiration,
//                    Token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token),
//                    UserId = user.UserId
//                });
//        }
//    }
//}

