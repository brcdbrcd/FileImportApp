using FileImportApp.API.Models.Client;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FileImportApp.API.Models.DB
{
    public class StoreItem
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string Key { get; set; }
        public string ArtikelCode { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string DeliveredIn { get; set; }
        public string Q1 { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Session { get; set; }

        public StoreItem(StoreItemDto rec, string _UploadedFileName, string _CreatedFileName)
        {
            Key = rec.Key;
            ArtikelCode = rec.ArtikelCode;
            ColorCode = rec.ColorCode;
            Description = rec.Description;
            Price = rec.Price;
            DiscountPrice = rec.DiscountPrice;
            DeliveredIn = rec.DeliveredIn;
            Q1 = rec.Q1;
            Size = rec.Size;
            Color = rec.Color;
        }
    }
}
