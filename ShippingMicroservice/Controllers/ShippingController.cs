using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ShippingMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public ShippingController(FirestoreDb db)
        {
            _db = db;
        }

        // POST: api/Shipping
        [HttpPost]
        public async Task<ActionResult<Shipping>> CreateShipping([FromBody] Shipping shipping)
        {
            var docRef = _db.Collection("shippings").Document(shipping.ShippingId);
            await docRef.SetAsync(shipping);
            return CreatedAtAction("GetShipping", new { id = docRef.Id }, shipping);
        }

        // GET: api/Shipping
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetShippings(string customerId)
        {
            var snapshot = await _db.Collection("shippings").WhereEqualTo("CustomerId", customerId).GetSnapshotAsync();
            var shippings = new List<Shipping>();
            foreach (var doc in snapshot)
            {
                shippings.Add(doc.ConvertTo<Shipping>());
            }
            return shippings;
        }


        // GET: api/Shipping/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShipping(string id)
        {
            var snapshot = await _db.Collection("shippings").Document(id).GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                return NotFound();
            }
            return snapshot.ConvertTo<Shipping>();
        }

        // PUT: api/Shipping/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipping(string id, [FromBody] Shipping shipping)
        {
            var snapshot = await _db.Collection("shippings").Document(id).GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                return NotFound();
            }
            await _db.Collection("shippings").Document(id).UpdateAsync("ShippingStatus", shipping.ShippingStatus);
            return NoContent();
        }

    }
}
