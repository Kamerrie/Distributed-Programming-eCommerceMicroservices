using Google.Cloud.Firestore;
using System;

namespace ShippingMicroservice.Models
{
    [FirestoreData]
    public class Shipping
    {
        [FirestoreProperty]
        public string ShippingId { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string OrderId { get; set; }

        [FirestoreProperty]
        public string CustomerId { get; set; }

        [FirestoreProperty]
        public string PaymentId { get; set; }

        [FirestoreProperty]
        public string ShippingAddress { get; set; }

        [FirestoreProperty]
        public string ShippingMethod { get; set; }

        [FirestoreProperty]
        public string ShippingStatus { get; set; } = "Order received, not yet dispatched";
    }
}
