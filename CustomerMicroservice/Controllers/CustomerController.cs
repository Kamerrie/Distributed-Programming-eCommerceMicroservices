using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using CustomerMicroservice.Models;

namespace CustomerMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FirebaseAuth _auth;
        private readonly FirebaseClient _client;

        public CustomerController()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("path/to/your/firebase/credentials.json"),
            });

            _auth = FirebaseAuth.DefaultInstance;
            _client = new FirebaseClient("https://your-database-url.firebaseio.com/");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var userRecordArgs = new UserRecordArgs()
            {
                Email = user.Email,
                EmailVerified = false,
                Password = user.Password,
                Disabled = false,
            };

            UserRecord userRecord = await _auth.CreateUserAsync(userRecordArgs);
            user.UserId = userRecord.Uid;

            // Save userRecord to your database
            var userToAdd = new User()
            {
                UserId = userRecord.Uid,
                Email = user.Email,
                Password = user.Password,
                Notifications = new List<Notification>()
            };

            await _client
                .Child("Users")
                .PostAsync(userToAdd);

            return Ok(userRecord);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check user credentials against your database and issue a token if they're valid
            User user = await _client
                .Child("Users")
                .OnceSingleAsync<User>();

            if (user.Email == email && user.Password == password)
            {
                // User credentials are valid, issue a token
                return Ok(new { token = "some token" });
            }

            return Unauthorized();
        }

        [HttpGet("notifications/{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            User user = await _client
                .Child("Users")
                .Child(userId)
                .OnceSingleAsync<User>();

            return Ok(user.Notifications);
        }

        [HttpPost("notifications/{userId}")]
        public async Task<IActionResult> AddNotification(string userId, Notification notification)
        {
            await _client
                .Child("Users")
                .Child(userId)
                .Child("Notifications")
                .PostAsync(notification);

            return Ok();
        }
    }
}
