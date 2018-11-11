using FileImportApp.API.Models.DB;

namespace FileImportApp.API.Models.Client
{
    public class StoreItemDto
    {
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

        public StoreItemDto(){}
        
        public StoreItemDto(StoreItem rec)
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
