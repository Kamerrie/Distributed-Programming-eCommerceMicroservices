using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace CustomerMicroservice.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string UserId { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public List<Notification> Notifications { get; set; }
    }
}
