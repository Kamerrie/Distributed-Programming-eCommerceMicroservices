using Google.Cloud.Firestore;
using System;

namespace ProductCatalogueMicroservice.Models
{
    [FirestoreData]
    public class Product
    {
        [FirestoreProperty]
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string Type { get; set; }

        [FirestoreProperty]
        public int Position { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Image { get; set; }

        [FirestoreProperty]
        public bool Has_Prime { get; set; }

        [FirestoreProperty]
        public bool Is_Best_Seller { get; set; }

        [FirestoreProperty]
        public bool Is_Amazon_Choice { get; set; }

        [FirestoreProperty]
        public bool Is_Limited_Deal { get; set; }

        [FirestoreProperty]
        public double? Stars { get; set; }

        [FirestoreProperty]
        public int? Total_Reviews { get; set; }

        [FirestoreProperty]
        public string Url { get; set; }

        [FirestoreProperty]
        public int? Availability_Quantity { get; set; }

        [FirestoreProperty]
        public string Price_String { get; set; }

        [FirestoreProperty]
        public string Price_Symbol { get; set; }

        [FirestoreProperty]
        public double? Price { get; set; }
    }

}
