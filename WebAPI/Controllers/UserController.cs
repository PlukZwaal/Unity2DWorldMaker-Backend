using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Register method
        [HttpPost("register")]
        public async Task<ActionResult> Register(User user)
        {
            // Check if user already exists
            if (await _userRepository.UserExistsAsync(user.Username))
            {
                return Conflict("User already exists");
            }

            // Hash the password before saving
            user.Password = HashPassword(user.Password);

            // Save user to the database
            await _userRepository.AddUserAsync(user);

            return Ok("User registered successfully.");
        }

        // Login method
        [HttpPost("login")]
        public async Task<ActionResult> Login(User user)
        {
            // Find user by username
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser == null)
            {
                return Unauthorized("User not found.");
            }

            // Verify the password
            if (VerifyPassword(user.Password, existingUser.Password))
            {
                return Ok("Login successful.");
            }

            return Unauthorized("Incorrect password.");
        }

        // Helper method to hash passwords
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Helper method to verify hashed passwords
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }
    }
}
