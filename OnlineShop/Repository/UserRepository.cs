using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace OnlineShop.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<(int, string)> CreateUser(RegistrationModel model, string role)
        {
            role.ToLower();
            //string userRole = GetRole(role);
            IdentityRole? roleVar = await roleManager.FindByNameAsync(role);
            if (roleVar == null)
            {
                return (0, "User role is invalid, please enter correct role");
            }
            User? userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");
            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name
            };

            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return (0, "User creation failed! Please check user details and try again.");


            await userManager.AddToRoleAsync(user, roleVar.Name != null ? roleVar.Name.ToString() : string.Empty);
            return (1, "User created successfully! with role: " + roleVar.Name);
        }


        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (0, "Invalid username");
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Invalid password");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return (1, token);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<(int, string)> DeleteUserAsync(string userEmail)
        {
            User? user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return (0, $"User with email {userEmail} not found");
            await userManager.DeleteAsync(user);
            return (1, $"User with email {userEmail} is Deleted");
        }
         
        public async Task<List<User>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<User?> GetAsync(string? email)
        {
            if (email == null)
            {
                return null;
            }
            return await this.userManager.FindByEmailAsync(email);
        }

        public async Task<(int, string)> UpdatePasswordAsync(RegistrationModel model, string newPassword)
        {
            User? exsistingUser = await userManager.FindByEmailAsync(model.Email);
            if (exsistingUser == null)
                return (0, $"User does not exsists with name {model.Email}");
            string correctPasswordHash = string.Empty;
            if (exsistingUser.PasswordHash is not null)
                correctPasswordHash = exsistingUser.PasswordHash;
            else
                return (0, $"Password hash for {model.Email} not found");
            // Create an instance of PasswordHasher
            var passwordHasher = new PasswordHasher<User>();
            // Verify the entered password against the stored hash
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(exsistingUser, correctPasswordHash, model.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return (0, "Incorrect password entered for " + model.Email);
            }
            exsistingUser.UserName = model.Username;
            exsistingUser.Name = model.Name;
            try
            {
                await userManager.UpdateAsync(exsistingUser);
                // Change the password
                var changePasswordResult = await userManager.ChangePasswordAsync(exsistingUser, model.Password, newPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return (0, "Password change failed: " + string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)));
                }
                return (1, $"User successfully Updated ! with email: {model.Email} ");
            }
            catch (Exception ex)
            {
                return (0, "User updation failed " + ex);
            }
        }

    }
}
