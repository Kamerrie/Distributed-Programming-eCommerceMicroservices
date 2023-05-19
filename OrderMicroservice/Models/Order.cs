using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace OrderMicroservice.Models
{
    [FirestoreData]
    public class Order
    {
        [FirestoreProperty]
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string CustomerId { get; set; }

        [FirestoreProperty]
        public List<Product> Products { get; set; }

        [FirestoreProperty]
        public double TotalPrice { get; set; }
    }
}
