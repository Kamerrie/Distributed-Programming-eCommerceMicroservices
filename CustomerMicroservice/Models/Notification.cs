using Google.Cloud.Firestore;
using System;

namespace CustomerMicroservice.Models
{
    [FirestoreData]
    public class Notification
    {
        [FirestoreProperty]
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public DateTime DateTimeUtc { get; set; } = DateTime.UtcNow;
    }
}
