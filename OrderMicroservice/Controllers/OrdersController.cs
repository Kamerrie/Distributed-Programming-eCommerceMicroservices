using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public OrdersController(FirestoreDb db)
        {
            _db = db;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(string customerId, List<string> productIds)
        {
            // Retrieve the products from Firestore
            var products = new List<Product>();
            foreach (var productId in productIds)
            {
                var productDoc = await _db.Collection("products").Document(productId).GetSnapshotAsync();
                if (!productDoc.Exists)
                {
                    return NotFound($"Product with ID [{productId}] does not exist.");
                }
                var product = productDoc.ConvertTo<Product>();
                products.Add(product);
            }

            // Create the order
            var order = new Order
            {
                CustomerId = customerId,
                Products = products,
                TotalPrice = products.Sum(product => product.Price)
            };

            var docRef = _db.Collection("orders").Document(order.OrderId);
            await docRef.SetAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = docRef.Id }, order);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var document = await _db.Collection("orders").Document(id).GetSnapshotAsync();

            if (!document.Exists)
            {
                return NotFound();
            }

            var order = document.ConvertTo<Order>();
            order.OrderId = document.Id;

            return order;
        }
    }
}
