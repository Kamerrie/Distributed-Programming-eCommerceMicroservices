using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentMicroservice.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public PaymentController(FirestoreDb db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
        {
            var docRef = _db.Collection("payments").Document(payment.PaymentId);
            await docRef.SetAsync(payment);

            return Ok(payment);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments(string customerId)
        {
            // Create a reference to the payments collection
            CollectionReference paymentsRef = _db.Collection("payments");

            // Create a query against the collection
            Query query = paymentsRef.WhereEqualTo("CustomerId", customerId);

            // Get the snapshot of the query result
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Convert the documents in the snapshot to Payment objects
            List<Payment> payments = new List<Payment>();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> payment = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(payment);
                    payments.Add(JsonConvert.DeserializeObject<Payment>(json));
                }
            }

            return Ok(payments);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(string id)
        {
            DocumentReference docRef = _db.Collection("payments").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound();
            }

            return snapshot.ConvertTo<Payment>();
        }
    }
}
