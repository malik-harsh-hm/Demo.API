using Demo.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private UserModel? _user;

        public AccountRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel data)
        {
            var user = new UserModel()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                UserName = data.Email
            };

            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded && data.Roles?.Count != 0)
                await _userManager.AddToRolesAsync(user, data.Roles);

            return result;
        }

        public async Task<string> SignInAsync(SignInModel data)
        {
            var result = await ValidateUser(data);
            if (result)
            {
                var generatedToken = await CreateToken();
                return generatedToken;
            }
            return null;
        }
        private async Task<bool> ValidateUser(SignInModel data)
        {
            _user = await _userManager.FindByNameAsync(data.Email);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, data.Password));
            return result;
        }
        private async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
