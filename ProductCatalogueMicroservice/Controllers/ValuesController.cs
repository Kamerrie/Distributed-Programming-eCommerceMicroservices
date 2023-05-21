using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCatalogueMicroservice.Models;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace ProductCatalogueMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public ProductController(FirestoreDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string category)
        {
            // Make a request to the Amazon API
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://amazon-data-scraper124.p.rapidapi.com/search/{category}?api_key=11f6bbe29358ea73c6e49238b21e43ea"),
                Headers =
            {
                { "X-RapidAPI-Key", "372ae50e9cmsh8c1ad7cebf433b5p10209ajsnabc0dde9900a" },
                { "X-RapidAPI-Host", "amazon-data-scraper124.p.rapidapi.com" },
            },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            // Deserialize the response
            var productsData = JsonConvert.DeserializeObject<ProductsData>(body);

            // Add each product to Firestore
            foreach (var product in productsData.Results)
            {
                var docRef = _db.Collection("products").Document(product.ProductId);
                await docRef.SetAsync(product);
            }

            // Return the products
            return Ok(productsData);
        }
    }
}
