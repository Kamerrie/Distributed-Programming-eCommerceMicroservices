using Google.Cloud.Firestore;

namespace CustomerMicroservice.Models
{
    [FirestoreData]
    public class Notification
    {
        [FirestoreProperty]
        public string NotificationId { get; set; }

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public bool IsRead { get; set; }
    }
}
