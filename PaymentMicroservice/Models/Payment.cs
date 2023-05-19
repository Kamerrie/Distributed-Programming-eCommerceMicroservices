using Google.Cloud.Firestore;
using System;

namespace PaymentMicroservice.Models
{
    [FirestoreData]
    public class Payment
    {
        [FirestoreProperty]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string OrderId { get; set; }

        [FirestoreProperty]
        public string CustomerId { get; set; }

        [FirestoreProperty]
        public double Amount { get; set; }

        // Credit Card Details
        [FirestoreProperty]
        public string CardHolderName { get; set; }

        [FirestoreProperty]
        public string CardNumber { get; set; }

        [FirestoreProperty]
        public string ExpiryMonth { get; set; }

        [FirestoreProperty]
        public string ExpiryYear { get; set; }

        [FirestoreProperty]
        public string CVV { get; set; }
    }
}
