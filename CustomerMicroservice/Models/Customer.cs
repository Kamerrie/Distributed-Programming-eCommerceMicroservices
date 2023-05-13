using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerMicroservice.Models
{
    [FirestoreData]
    public class Customer
    {
        [FirestoreProperty]
        [Required]
        public string CustomerId { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public List<Notification> Notifications { get; set; }
    }
}
