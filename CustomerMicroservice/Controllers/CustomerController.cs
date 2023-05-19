using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CustomerMicroservice.Models;
using Grpc.Auth;
using System.Collections.Generic;

namespace CustomerMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public CustomerController(FirestoreDb db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            DocumentReference docRef = _db.Collection("Customers").Document(customer.CustomerId);
            await docRef.SetAsync(customer);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Customer customer)
        {
            Query query = _db.Collection("Customers")
                .WhereEqualTo("Email", customer.Email)
                .WhereEqualTo("Password", customer.Password);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Count > 0)
            {
                return Ok(true);
            }

            // If no documents were found, return false
            return Ok(false);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            DocumentReference docRef = _db.Collection("Customers").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound();
            }

            Customer customer = snapshot.ConvertTo<Customer>();
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Customer customer)
        {
            DocumentReference docRef = _db.Collection("Customers").Document(id);
            await docRef.SetAsync(customer, SetOptions.MergeAll);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            DocumentReference docRef = _db.Collection("Customers").Document(id);
            await docRef.DeleteAsync();
            return NoContent();
        }

        [HttpPost("users/{userId}/notifications")]
        public async Task<IActionResult> AddNotification(string id, Notification notification)
        {
            // Fetch the user's document reference
            DocumentReference docRef = _db.Collection("Customers").Document(id);

            // Fetch the user's data
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound("User not found");
            }

            // Add the new notification to the user's Notifications list
            Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "Notifications", FieldValue.ArrayUnion(notification) }
                };

            // Update the user document with the new Notifications list
            await docRef.UpdateAsync(updates);

            return Ok(notification);
        }

    }
}
